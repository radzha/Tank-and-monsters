using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Progress;
using System.Linq;

/// <summary>
/// Менеджер производителей юнитов.
/// </summary>
public class SpawnersManager : Singleton<SpawnersManager> {
	// Префабы юнитов.
	[System.Serializable]
	public struct unitPrefabs {
		public bool isEnemy;
		public Settings.Unit.UnitType type;
		public int weight;
		public GameObject prefab;
	}

	// Массив префабов юнитов.
	public unitPrefabs[] UnitPrefabs;
	// Максимум врагов на поле.
	public int enemySpawnLimit = 10;
	// Максимум миньонов на поле.
	public int mignonSpawnLimit = 10;
	// Стартовая задержка производства юнитов.
	public int startDelay = 10;
	// Количество волн врагов.
	public int enemyWavesCount = 3;
	// Длительность одной волны врагов.
	public int enemyWaveDuration = 20;
	// Промежуток между волнами врагов.
	public int enemyWaveDelay = 10;
	// Все казармы.
	public HashSet<Spawner> spawners;

	// Если выделен юнит.
	public delegate void OnUnitSelected(Selectable thing);

	public OnUnitSelected onUnitSelected = delegate {
	};

	// Текущая волна атаки.
	private int currentWave;
	// Таймер стартового ожидания и паузы между волнами врагов.
	private float delayTimer;
	// Таймер продолжительности волн.
	private float waveTimer = -1f;

	// Пул свободных юнитов.
	private HashSet<GameObject> unitsPool = new HashSet<GameObject>();

	/// <summary>
	/// Является ли волна атаки последней.
	/// </summary>
	public bool IsLastWave {
		get {
			return currentWave >= enemyWavesCount - 1;
		}
	}

	// Произведенные юниты.
	public HashSet<Unit> Units {
		get;
		private set;
	}

	// Возвращает все выделенные юниты.
	public IEnumerable<Unit> UnitsSelected {
		get {
			return Units.Where(u => u.IsSelected());
		}
	}

	/// <summary>
	/// Все объекты, доступные для выделения.
	/// </summary>
	public HashSet<Selectable> AllSelectable {
		get {
			var set = new HashSet<Selectable>();
			foreach (var unit in Units) {
				set.Add(unit);
			}
			foreach (var spawner in spawners) {
				set.Add(spawner);
			}
			return set;
		}
	}

	protected void Awake() {
		Units = new HashSet<Unit>();
		spawners = new HashSet<Spawner>(FindObjectsOfType<Spawner>());
		StartCoroutine(TimeControl());
	}

	/// <summary>
	/// Управление волнами врагов.
	/// </summary>
	private IEnumerator TimeControl() {
		delayTimer = startDelay;
		while (delayTimer > 0f) {
			delayTimer -= Time.deltaTime;
			yield return null;
		}
		while (currentWave < enemyWavesCount) {
			waveTimer = enemyWaveDuration;
			while (waveTimer > 0f) {
				waveTimer -= Time.deltaTime;
				yield return null;
			}
			delayTimer = enemyWaveDelay;
			while (delayTimer > 0f) {
				delayTimer -= Time.deltaTime;
				yield return null;
			}
			currentWave++;
		}
	}

	/// <summary>
	/// Возвращает главного персонажа или null, если он не создан.
	/// </summary>
	public MainCharacter MainCharacter() {
		return Units.FirstOrDefault(u => u.unitType == Settings.Unit.UnitType.Player) as MainCharacter;
	}

	/// <summary>
	/// Возвращает миньонов.
	/// </summary>
	public HashSet<Unit> Mignons() {
		return new HashSet<Unit>(Units.Where(u => !u.IsEnemy));
	}

	/// <summary>
	/// Количество миньонов.
	/// </summary>
	public int MignonsCount() {
		return Units.Count(u => !u.IsEnemy);
	}

	/// <summary>
	/// Возвращает врагов.
	/// </summary>
	public HashSet<Unit> Enemies() {
		return new HashSet<Unit>(Units.Where(u => u.IsEnemy));
	}

	/// <summary>
	/// Количество врагов.
	/// </summary>
	public int EnemiesCount() {
		return Units.Count(u => u.IsEnemy);
	}

	/// <summary>
	/// Добавить юнит в сет.
	/// </summary>
	/// <param name="unit"></param>
	public void AddUnit(Unit unit) {
		Units.Add(unit);
	}

	/// <summary>
	/// Разрешает/запрещает делать юнитов.
	/// </summary>
	public bool CanSpawn(bool isEnemy) {
		return waveTimer > 0f && (isEnemy ? EnemiesCount() < enemySpawnLimit : MignonsCount() < mignonSpawnLimit);
	}

	/// <summary>
	/// Выделить объект и убрать выделение с других.
	/// </summary>
	public void Select(Selectable thing) {
		foreach (var t in AllSelectable) {
			t.SetSelected(t.Equals(thing));
		}
	}

	/// <summary>
	/// Удалить юнит из числа активных и добавить его в пул.
	/// </summary>
	/// <param name="unit">Unit.</param>
	public void RemoveUnit(Unit unit) {
		Units.Remove(unit);
		unit.gameObject.SetActive(false);
		unitsPool.Add(unit.gameObject);
	}

	/// <summary>
	/// Возвращает новый экземпляр юнита.
	/// Если пул не пуст берёт из пула.
	/// </summary>
	/// <returns>The new unit.</returns>
	/// <param name="isEnemy">If set to <c>true</c> is enemy.</param>
	/// <param name="type">Type.</param>
	/// <param name="spawnPoint">Spawn point.</param>
	public GameObject GetNewUnit(bool isEnemy, Settings.Unit.UnitType type, Vector3 spawnPoint) {
		if (unitsPool.Count == 0) {
			var prefab = SpawnersManager.Instance.UnitPrefabs
				.First(u => u.isEnemy == isEnemy && u.type == type)
				.prefab;
			return (GameObject)Instantiate(prefab, spawnPoint, Quaternion.identity);
		} else {
			var unit = unitsPool.First();
			unitsPool.Remove(unit);
			unit.transform.position = spawnPoint;
			unit.GetComponent<Unit>().SetUnit();
			unit.SetActive(true);
			return unit;
		}
	}
}
