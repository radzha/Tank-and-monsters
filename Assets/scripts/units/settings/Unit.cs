namespace Settings {
	/// <summary>
	/// Базовый класс юнита
	/// </summary>
	public class Unit {
		public enum UnitType {
			// Воин ближнего боя
			Warrior,
			// Воин дальнего боя
			Archer,
			// Босс
			Boss,
			// Игрок
			Player
		}

		private int hp;
		private float armor;
		private int attack;
		private float attackSpeed;
		private float speed;
		private float attackRange;
		private int gold;
		private int xp;

		/// <summary>
		/// Количество жизней
		/// </summary>
		public int Hp {
			get {
				return hp;
			}
			set {
				hp = value;
			}
		}

		/// <summary>
		/// Броня (Armor) — сколько урона в процентном соотношении может быть поглощено
		/// </summary>
		public float Armor {
			get {
				return armor;
			}
			set {
				armor = value;
			}
		}

		/// <summary>
		/// Показатель атаки (Attack) — сколько урона наносится за один удар
		/// </summary>
		public int Attack {
			get {
				return attack;
			}
			set {
				attack = value;
			}
		}

		/// <summary>
		/// Скорость атаки (Attack Speed) — ударов в секунду
		/// </summary>
		public float AttackSpeed {
			get {
				return attackSpeed;
			}
			set {
				attackSpeed = value;
			}
		}

		/// <summary>
		/// Скорость передвижения (Speed) — метров в секунду
		/// </summary>
		public float Speed {
			get {
				return speed;
			}
			set {
				speed = value;
			}
		}

		/// <summary>
		/// Дальность атаки (Attack Range) — расстояние, на которое необходимо подойти, чтобы нанести урон
		/// </summary>
		public float AttackRange {
			get {
				return attackRange;
			}
			set {
				attackRange = value;
			}
		}

		/// <summary>
		/// Количество золота (Gold) — количество золота, которое получает игрок за убийство юнита
		/// </summary>
		public int Gold {
			get {
				return gold;
			}
			set {
				gold = value;
			}
		}

		/// <summary>
		/// Количество опыта (XP) — количество опыта, которое получает главный персонаж игрока за убийство юнита.
		/// Для главного персонажа это кол-во опыта, необходимое для увеличения уровня.
		/// </summary>
		public int Xp {
			get {
				return xp;
			}
			set {
				xp = value;
			}
		}

		/// <summary>
		/// Прочитать настройки из редактора уровней.
		/// </summary>
		/// <param name="unitSettings">Набор настроек.</param>
		/// <param name="level">Уровень.</param>
		private void ReadSettings(LevelEditor.UnitSettings[] unitSettings, int level) {
			var unit = unitSettings[level];
			Hp = unit.hp;
			Armor = unit.armor;
			Attack = unit.attack;
			AttackSpeed = unit.attackSpeed;
			Speed = unit.speed;
			AttackRange = unit.attackRange;
			Xp = unit.xp;
			Gold = unit.gold;
		}

		/// <summary>
		/// Первичное заполнение настроек в зависимости от типа и принадлежности юнита.
		/// </summary>
		public Unit(UnitType type, bool isEnemy, int level = 0) {
			if (isEnemy) {
				switch (type) {
				case UnitType.Warrior:
					ReadSettings(LevelEditor.Instance.enemyWarrior, level);
					break;
				case UnitType.Archer:
					ReadSettings(LevelEditor.Instance.enemyArcher, level);
					break;
				case UnitType.Boss:
					ReadSettings(LevelEditor.Instance.boss, level);
					break;
				}
			} else {
				switch (type) {
				case UnitType.Warrior:
					ReadSettings(LevelEditor.Instance.warrior, level);
					break;
				case UnitType.Archer:
					ReadSettings(LevelEditor.Instance.archer, level);
					break;
				case UnitType.Player:
					ReadSettings(LevelEditor.Instance.player, level);
					break;
				}
			}
		}
	}
}
