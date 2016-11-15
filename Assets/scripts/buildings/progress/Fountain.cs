using UnityEngine;

namespace Progress {
	/// <summary>
	/// Фонтан. Умеет порождать героя и восстанавливать здоровье.
	/// </summary>
	public class Fountain : Singleton<Fountain>, Damagable {

		// Настройки фонтана.
		private Settings.Fountain settings;

		void Awake() {
			settings = new Settings.Fountain();
		}

		/// <summary>
		/// Принимает урон и дает профит.
		/// Конкретно фонтан неубиваем, а возвращаемый профит - здоровье.
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="damage"></param>
		/// <returns></returns>
		public Unit.Profit TakeDamage(Unit unit, float damage) {
			return unit is MainCharacter ? new Unit.Profit(settings.PlayerCureSpeed, 0, 0) : new Unit.Profit(settings.MignonCureSpeed, 0, 0);
		}

		public Unit.Profit TakeDamage(Unit unit, float damage, float slow, float attackSlow, float duration) {
			return TakeDamage(unit, damage);
		}

		/// <summary>
		/// Текущее здоровье.
		/// </summary>
		public int Health() {
			return int.MaxValue;
		}

		/// <summary>
		/// Максимальное здоровье.
		/// </summary>
		public int MaxHealth() {
			return int.MaxValue;
		}

		/// <summary>
		/// Смерть. Но фонтан неразрушаем.
		/// </summary>
		public void OnDie() {
		}

		/// <summary>
		/// Фонтан всегда жив.
		/// </summary>
		public bool IsDead() {
			return false;
		}

		/// <summary>
		/// Юнит подошел к фонтану.
		/// </summary>
		private void OnTriggerEnter(Collider other) {
			var unit = other.gameObject.GetComponent<Unit>();
			if (unit != null && this.Equals(unit.target.aim)) {
				unit.AimTriggered = true;
			}
		}

		/// <summary>
		/// Юнит ушел из фонтана.
		/// </summary>
		private void OnTriggerExit(Collider other) {
			var unit = other.gameObject.GetComponent<Unit>();
			if (unit != null) {
				unit.AimTriggered = false;
			}
		}
	}
}
