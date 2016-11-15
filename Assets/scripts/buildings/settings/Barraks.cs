namespace Settings {
	/// <summary>
	/// Казармы
	/// </summary>
	public class Barraks {
		/// <summary>
		/// Скорость тренировки - юнитов в секунду
		/// </summary>
		public float TrainingSpeed {
			get; set;
		}

		/// <summary>
		/// Уровень казармы
		/// </summary>
		public int Level {
			get; set;
		}

		/// <summary>
		/// Тип казармы.
		/// </summary>
		public Unit.UnitType Type {
			get; set;
		}
	}
}
