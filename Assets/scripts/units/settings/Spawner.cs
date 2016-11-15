namespace Settings {
	/// <summary>
	/// Настройки казарм.
	/// </summary>
	public class Spawner {
		/// <summary>
		/// Количество золота (Gold) — количество золота, требуемое для апгрейда казармы.
		/// </summary>
		public int Gold {
			get; set;
		}

		/// <summary>
		/// Скорость производства юнитов, шт./сек.
		/// </summary>
		public float SpawnSpeed {
			get; set;
		}

		/// <summary>
		/// Прочитать настройки из редактора уровней.
		/// </summary>
		/// <param name="settings">Набор настроек.</param>
		/// <param name="level">Уровень.</param>
		private void ReadSettings(LevelEditor.Spawner[] settings, int level) {
			var spawner = settings[level];
			Gold = spawner.gold;
			SpawnSpeed = spawner.spawnSpeed;
		}

		/// <summary>
		/// Первичное заполнение настроек.
		/// </summary>
		public Spawner(int level = 0) {
			ReadSettings(LevelEditor.Instance.spawner, level);
		}

	}
}
