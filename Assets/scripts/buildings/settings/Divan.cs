namespace Settings {
	/// <summary>
	/// Настройки дивана
	/// </summary>
	public class Divan {
		/// Количество жизней.
		public int Hp {
			get;
			private set;
		}

		public Divan() {
			Hp = LevelEditor.Instance.DivanHealth;
		}
	}
}
