namespace Progress {
	/// <summary>
	/// Класс дивана - основной цели врагов.
	/// </summary>
	public class Divan : Singleton<Divan>, Damagable {

		// Индикатор окончания игры.
		public static bool gameStop;

		public delegate void onGameEnd(bool win);

		public onGameEnd OnGameEnd;

		// Настройки дивана.
		private Settings.Divan settings;

		// Текущее здоровье.
		private int health;

		void Awake() {
			settings = new Settings.Divan();
			health = MaxHealth();
		}

		/// <summary>
		/// Текущее здоровье.
		/// </summary>
		public int Health() {
			return health;
		}

		/// <summary>
		/// Максимальное здоровье.
		/// </summary>
		public int MaxHealth() {
			return settings.Hp;
		}

		/// <summary>
		/// Принять урон. Возвратить профит.
		/// </summary>
		public Unit.Profit TakeDamage(Unit unit, float damage) {
			if (IsDead()) {
				return new Unit.Profit(0, 0, 0);
			}
			health -= (int)damage;
			if (health <= 0) {
				OnDie();
			}
			return new Unit.Profit(0, 0, 0);
		}

		/// <summary>
		/// Урон от абилки. Не актуален для дивана.
		/// </summary>
		public Unit.Profit TakeDamage(Unit unit, float damage, float slow, float attackSlow, float duration) {
			return TakeDamage(unit, damage);
		}

		/// <summary>
		/// Смерть дивана вызывает остановку игры.
		/// </summary>
		public void OnDie() {
			gameObject.SetActive(false);
			OnGameEnd(false);
			gameStop = true;
		}

		/// <summary>
		/// Убит ли диван.
		/// </summary>
		public bool IsDead() {
			return health <= 0;
		}

	}
}
