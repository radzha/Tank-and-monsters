namespace Progress {
	/// <summary>
	/// Класс игрока.
	/// </summary>
	public static class Player {
		private static int _experience;
		private static int _level;

		/// <summary>
		/// Текущий опыт.
		/// </summary>
		public static int Experience {
			get {
				return _experience;
			}
			set {
				_experience = value;
				if (value > LevelEditor.Instance.player [Level].xp) {
					Level++;
				}
			}
		}

		/// <summary>
		/// Достигнут ли максимальный уровень.
		/// </summary>
		private static bool IsLevelMax() {
			return Level >= LevelEditor.Instance.player.Length - 1;
		}

		/// <summary>
		/// Текущий уровень золота.
		/// </summary>
		public static int GoldAmount {
			get;
			set;
		}

		/// <summary>
		/// Уровень игрока.
		/// </summary>
		public static int Level {
			get {
				return _level;
			}
			set {
				if (IsLevelMax()) {
					return;
				}
				var oldValue = _level;
				_level = value;
				if (value > oldValue) {
					if (SpawnersManager.Instance.MainCharacter() != null) {
						SpawnersManager.Instance.MainCharacter().DoLevelUp();
					}
				}
			}
		}
	}
}