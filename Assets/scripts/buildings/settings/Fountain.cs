namespace Settings {
	/// <summary>
	/// Фонтан. Настройки.
	/// </summary>
	public class Fountain {
		/// <summary>
		/// Скорость восстановления жизней героя.
		/// </summary>
		public int PlayerCureSpeed {
			get;
			private set;		
		}

		/// <summary>
		/// Скорость восстановления жизней миньона.
		/// </summary>
		public int MignonCureSpeed {
			get;
			private set;
		}

		public Fountain() {
			PlayerCureSpeed = LevelEditor.Instance.fountain.playerCureSpeed;
			MignonCureSpeed = LevelEditor.Instance.fountain.mignonCureSpeed;
		}
	}
}
