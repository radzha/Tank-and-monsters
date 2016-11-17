using UnityEngine;
using System.Collections.Generic;
using Progress;
using System.Linq;

/// <summary>
/// Производитель юнитов.
/// </summary>
public class Spawner : MonoBehaviour, Selectable {
	// Скорость тренировки - юнитов в секунду.
	public float trainingSpeed;
	// Типы юнитов, которые можно здесь производить.
	public Settings.Unit.UnitType[] spawnUnitTypes = null;
	// Производить врагов или миньонов.
	public bool isEnemy = false;
	// Настройки.
	public Settings.Spawner settings;
	// Радиус, определяющий круг, в котором запрещён спаун.
	public float radius;

	// Веса, определяющие шансы производства того или иного типа юнита.
	private Dictionary<Settings.Unit.UnitType, float> typeChances;
	// Разброс расстояния точки появления юнита от базы.
	private readonly float length = 25f / Mathf.Sqrt(2);
	// Таймер длительности тренировки юнита.
	private float trainingTimer;
	// Выделена ли казарма.
	private bool selected;
	// Уровень казармы.
	private int level;
	// Основной цвет казармы.
	private Color defaultColor;

	// Уровень казармы.
	public int Level {
		get {
			return level;
		}
		set {
			if (value <= level || value > LevelEditor.Instance.spawner.Length - 1) {
				return;
			}
			level = value;
			OnLevelUp();
		}
	}

	private void Awake() {
		settings = new Settings.Spawner(Level);
		trainingSpeed = spawnUnitTypes[0] == Settings.Unit.UnitType.Player ? LevelEditor.Instance.playerRespawnTime[Level] : settings.SpawnSpeed;
		trainingTimer = spawnUnitTypes[0] == Settings.Unit.UnitType.Player ? 0f : trainingSpeed > 0f ? 1 / trainingSpeed : Mathf.Infinity;
		typeChances = new Dictionary<Settings.Unit.UnitType, float>();
		var weightSum = SpawnersManager.Instance.UnitPrefabs.Where(u => u.isEnemy == isEnemy && spawnUnitTypes.Contains(u.type)).Sum(u => u.weight);
		foreach (var type in spawnUnitTypes) {
			var chance = SpawnersManager.Instance.UnitPrefabs.First(u => u.isEnemy == isEnemy && u.type == type).weight / (float)weightSum;
			typeChances.Add(type, chance);
		}
		var material = GetComponent<Renderer>().material;
		defaultColor = material.GetColor("_EmissionColor");
	}

	/// <summary>
	/// Вычислияет время тренировки юнита.
	/// </summary>
	public int RespawnTime() {
		if (spawnUnitTypes[0] == Settings.Unit.UnitType.Player) {
			return SpawnersManager.Instance.MainCharacter() != null ? 0 : trainingTimer > 0f ? (int)trainingTimer : 0;
		}
		return 0;
	}

	/// <summary>
	/// Является ли казарма - производителем героя. Он всегда единственный тип.
	/// </summary>
	/// <returns></returns>
	private bool IsMainCharacterSpawner() {
		return spawnUnitTypes[0] == Settings.Unit.UnitType.Player;
	}

	private void Update() {
		if (Divan.gameStop) {
			return;
		}

		if (IsMainCharacterSpawner() && SpawnersManager.Instance.MainCharacter() != null) {
			return;
		}

		if (trainingTimer > 0f) {
			trainingTimer -= Time.deltaTime;
			return;
		}
		trainingTimer = trainingSpeed > 0f ? 1 / trainingSpeed : Mathf.Infinity;

		if (!IsMainCharacterSpawner() && !SpawnersManager.Instance.CanSpawn(isEnemy)) {
			return;
		}

		MakeUnit();
	}

	/// <summary>
	/// Создать юнит.
	/// </summary>
	private void MakeUnit() {
		var type = RandomType();
		var rand = Random.Range(radius, length);
		rand *= Mathf.Sign(Random.Range(-1f, 1f));
		// случайный разброс 
		var spawnPoint = new Vector3(transform.position.x + rand, 0f, transform.position.z + rand);
		// рождение юнита в случайном месте споунера
		var prefab = SpawnersManager.Instance.UnitPrefabs.First(u => u.isEnemy == isEnemy && u.type == type).prefab;
		var unit = (GameObject)Instantiate(prefab, spawnPoint, Quaternion.identity);
		var unitComp = unit.GetComponent<Unit>();
		unitComp.Level = Level;
		unit.transform.position = new Vector3(unit.transform.position.x, unit.transform.localScale.y, unit.transform.position.z);
		SpawnersManager.Instance.AddUnit(unitComp);
	}

	/// <summary>
	/// Возвратить случайный тип юнита на основе весов.
	/// </summary>
	/// <returns></returns>
	private Settings.Unit.UnitType RandomType() {
		var randForType = Random.Range(0, 1f);
		var sum = 0f;
		foreach (var typeChance in typeChances) {
			sum += typeChance.Value;
			if (randForType <= sum) {
				return typeChance.Key;
			}
		}
		return spawnUnitTypes.Last();
	}

	/// <summary>
	/// Выделить казарму.
	/// </summary>
	public void SetSelected(bool select) {
		selected = select;
		SpawnersManager.Instance.onUnitSelected(this);
		SelectVisually(selected);
	}

	/// <summary>
	/// Выделить/cнять выделение визуально.
	/// </summary>
	private void SelectVisually(bool select) {
		var material = GetComponent<Renderer>().material;
		var color = defaultColor;
		if (select) {
			defaultColor = material.GetColor("_EmissionColor");
			color = Color.cyan;
		}
		material.SetColor("_EmissionColor", color);
	}

	/// <summary>
	/// Выделена ли казарма.
	/// </summary>
	public bool IsSelected() {
		return selected;
	}

	/// <summary>
	/// Улучшить казарму, потратив золото.
	/// </summary>
	public void Upgrade() {
		if (settings.Gold <= Player.GoldAmount) {
			Level++;
			Player.GoldAmount -= settings.Gold;
			settings = new Settings.Spawner(Level);
		}
	}

	/// <summary>
	/// Срабатыает при повышении уровня казармы. 
	/// Вызывает повышения уровней уже сделанных юнитов.
	/// </summary>
	private void OnLevelUp() {
		var units = SpawnersManager.Instance.Units.Where(u => u.IsEnemy == isEnemy && spawnUnitTypes.Contains(u.unitType));
		foreach (var unit in units) {
			unit.Level++;
		}
	}

	/// <summary>
	/// Строка, описывающая тип и принадлежность казармы.
	/// </summary>
	public string PrettyType() {
		var type = "";
		switch (spawnUnitTypes[0]) {
			case Settings.Unit.UnitType.Archer:
				type = "стрелков";
				break;
			case Settings.Unit.UnitType.Warrior:
				type = "воинов";
				break;
			case Settings.Unit.UnitType.Boss:
				type = "босса";
				break;
			case Settings.Unit.UnitType.Player:
				type = "героя";
				break;
			default:
				throw new System.ArgumentOutOfRangeException();
		}
		return "Казарма " + type;
	}

}
