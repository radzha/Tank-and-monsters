using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Progress {
	/// <summary>
	/// Класс главного персонажа.
	/// </summary>
	public class MainCharacter : Unit {

		/// <summary>
		/// Перечень режимов атаки.
		/// </summary>
		public enum AttackMode {
			Normal,
			MeteoRain,
			IceArrow
		}

		// Иконка курсора ледяной стрелы.
		public Texture2D iceCursorTexture;

		/// <summary>
		/// У главного персонажа целью может быть просто точка (x,z).
		/// </summary>
		public Vector2 PositionTarget {
			get;
			set;
		}

		/// <summary>
		/// Режим цели - не юнит, а позиция.
		/// </summary>
		public bool PositionTargetMode {
			get;
			set;
		}

		/// <summary>
		/// Режим атаки - нормальный или абилкой.
		/// </summary>
		public AttackMode attackMode {
			get;
			set;
		}

		/// <summary>
		/// У основного персонажа уровень статический.
		/// </summary>
		public override int Level {
			get {
				return Player.Level;
			}
			set {
				Player.Level = value;
			}
		}

		/// <summary>
		/// Автоматически выбираемый таймер.
		/// </summary>
		protected override float AttackTimer {
			get {
				switch (attackMode) {
				case AttackMode.Normal:
					return base.AttackTimer;
				case AttackMode.MeteoRain:
					return meteoRainTimer;
				case AttackMode.IceArrow:
					return iceArrowTimer;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			set {
				switch (attackMode) {
				case AttackMode.Normal:
					base.AttackTimer = value;
					break;
				case AttackMode.MeteoRain:
					meteoRainTimer = value;
					break;
				case AttackMode.IceArrow:
					iceArrowTimer = value;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Таймер абилки ледяной стрелы.
		private float iceArrowTimer;
		// Таймер абилки метеоритного дождя.
		private float meteoRainTimer;

		protected override void Update() {
			base.Update();

			iceArrowTimer -= Time.deltaTime;
			meteoRainTimer -= Time.deltaTime;

			if (Input.GetKeyDown(KeyCode.Escape)) {
				TurnOffAbilities();
			}
			if (Input.GetKeyDown(KeyCode.L)) {
				PerformAbility(AttackMode.IceArrow);
			}
			if (Input.GetKeyDown(KeyCode.M)) {
				PerformAbility(AttackMode.MeteoRain);
			}
		}

		/// <summary>
		/// Выполнить абилку.
		/// </summary>
		/// <param name="mode">Абилка</param>
		public void PerformAbility(AttackMode mode) {
			switch (mode) {
			case AttackMode.MeteoRain:
				if (meteoRainTimer <= 0f && IsSelected()) {
					MeteoRainMode(attackMode != AttackMode.MeteoRain);
				}
				break;
			case AttackMode.IceArrow:
				if (iceArrowTimer <= 0f && IsSelected()) {
					IceArrowMode(attackMode != AttackMode.IceArrow);
				}
				break;
			}
		}

		/// <summary>
		/// Отключить все абилки.
		/// </summary>
		public void TurnOffAbilities() {
			IceArrowMode(false);
			MeteoRainMode(false);
		}

		/// <summary>
		/// Включить/выключить абилку ледяной стрелы.
		/// </summary>
		private void IceArrowMode(bool enable) {
			attackMode = enable ? AttackMode.IceArrow : AttackMode.Normal;
			if (!enable) {
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
				target.aim = null;
				return;
			}
			Cursor.SetCursor(iceCursorTexture, Vector2.zero, CursorMode.Auto);
		}

		/// <summary>
		/// Включить/выключить абилку метеоритного дождя.
		/// </summary>
		private void MeteoRainMode(bool enable) {
			var marker = MeteoRainMarkerManager.Instance.meteoRainMarker;
			if (marker == null) {
				return;
			}
			marker.gameObject.SetActive(enable);
			attackMode = enable ? AttackMode.MeteoRain : AttackMode.Normal;
			if (enable) {
				var x = LevelEditor.Instance.meteoRain[Player.Level].radius;
				marker.rectTransform.sizeDelta = new Vector2(2f * x, 2f * x);
				marker.transform.position = transform.position;
			}
		}

		/// <summary>
		/// В зоне ли поражения юнит, в зависимости от дистанции и режиме атаки.
		/// </summary>
		protected override bool IsInRange(float distance) {
			var normalAttack = attackMode == AttackMode.Normal && base.IsInRange(distance);
			var iceArrowAttack = attackMode == AttackMode.IceArrow && distance <= LevelEditor.Instance.iceArrow[Player.Level].radius;
			return normalAttack || iceArrowAttack;
		}

		/// <summary>
		/// Возвращает время перезарядки оружия.
		/// </summary>
		public override float CoolDown {
			get {
				switch (attackMode) {
				case AttackMode.Normal:
					return 1f / attackSpeed;
				case AttackMode.MeteoRain:
					return LevelEditor.Instance.meteoRain[Player.Level].cooldown;
				case AttackMode.IceArrow:
					return LevelEditor.Instance.iceArrow[Player.Level].cooldown;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		/// <summary>
		/// Непосредственный ущерб юниту, дивану или подпитка жизни из фонтана.
		/// </summary>
		public override void MakeDamage() {
			switch (attackMode) {
			case AttackMode.Normal:
				base.MakeDamage();
				break;
			case AttackMode.MeteoRain:
				MakeMeteoRainDamage();
				break;
			case AttackMode.IceArrow:
				if (target.aim != null) {
					target.aim.TakeDamage(
					this,
					Settings.Attack,
					LevelEditor.Instance.iceArrow[Player.Level].slow,
					LevelEditor.Instance.iceArrow[Player.Level].attackSlow,
					LevelEditor.Instance.iceArrow[Player.Level].duration);
					IceArrowMode(false);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Нанести урон абилкой метеоритного дождя.
		/// </summary>
		private void MakeMeteoRainDamage() {
			foreach (var enemy in SpawnersManager.Instance.Enemies().Where(enemy => enemy.IsInMeteoRainRange(LevelEditor.Instance.meteoRain[Player.Level].radius))) {
				enemy.TakeDamage(this, Settings.Attack);
				enemy.MeteoRainVisually(true);
			}
			MeteoRainMode(false);
		}

		/// <summary>
		/// Подготовить маркер выделения героя.
		/// </summary>
		protected override void PrepareSelectMarker() {
			base.PrepareSelectMarker();
			var image = selectMarker.transform.GetChild(0).GetComponent<Image>();
			image.color = Color.cyan;
		}

		/// <summary>
		/// Определить цель.
		/// </summary>
		protected override void DefineTarget() {
			// Главный персонаж не определяет цель автоматически.
		}

		/// <summary>
		/// Переместить юнит.
		/// </summary>
		protected override void Move() {
			base.Move();
			if (!PositionTargetMode) {
				return;
			}

			// Перемещение в режиме цели - не юнита, а точки.
			var myPos = new Vector2(transform.position.x, transform.position.z);
			var distance = Vector2.Distance(myPos, PositionTarget);
			var moveTo = Vector2.Lerp(myPos, PositionTarget, Settings.Speed * Time.deltaTime / distance);
			var y = transform.position.y;
			transform.position = new Vector3(moveTo.x, y, moveTo.y);

			// Повернуться в сторону цели.
			transform.LookAt(new Vector3(PositionTarget.x, y, PositionTarget.y));
			transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
		}

		/// <summary>
		/// При создании героя направить камеру на него и выделить.
		/// </summary>
		public void OnEnable() {
			CameraManager.Instance.AutoMove = true;
			SetSelected(true);
		}

		/// <summary>
		/// При смерти героя сменить режим камеры и выключить абилки.
		/// </summary>
		public void OnDisable() {
			CameraManager.Instance.AutoMove = false;
			IceArrowMode(false);
			MeteoRainMode(false);
		}

		/// <summary>
		/// Назначить/снять выделение юнита.
		/// </summary>
		public override void SetSelected(bool selected) {
			base.SetSelected(selected);
			MouseManager.Instance.IceArrowClickUI.SetActive(selected);
			MouseManager.Instance.MeteoRainClickUI.SetActive(selected);
		}

		/// <summary>
		/// Вызывается при повышения уровня.
		/// </summary>
		public void DoLevelUp() {
			SettingsRead();
			animator.SetTrigger("levelup");
		}

		/// <summary>
		/// Текст абилки для UI.
		/// </summary>
		public string IceArrowTimerString {
			get {
				return iceArrowTimer <= 0f ? "Готово" : string.Format("{0:###.#}", iceArrowTimer);
			}
		}

		/// <summary>
		/// Текст абилки для UI.
		/// </summary>
		public string MeteoRainTimerString {
			get {
				return meteoRainTimer <= 0f ? "Готово" : string.Format("{0:###.#}", meteoRainTimer);
			}
		}

	}
}
