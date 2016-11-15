using Progress;

/// <summary>
/// Интерфейс юнитов и зданий, позволяющий принимать урон и отдавать профит.
/// </summary>
public interface Damagable {
	/// <summary>
	/// Текущее здоровье.
	/// </summary>
	int Health();

	/// <summary>
	/// Максимум здоровья.
	/// </summary>
	int MaxHealth();

	/// <summary>
	/// Вызывается после смерти юнита или дивана.
	/// </summary>
	void OnDie();

	/// <summary>
	/// Жив ли юнит или здание.
	/// </summary>
	bool IsDead();

	/// <summary>
	/// Принять урон.
	/// </summary>
	/// <param name="unit">От кого урон.</param>
	/// <param name="damage">Сколько урона.</param>
	/// <returns>Профит наносящему урон.</returns>
	Unit.Profit TakeDamage(Unit unit, float damage);

	/// <summary>
	/// Принять урон от абилки (стрелы льда)
	/// </summary>
	/// <param name="unit">От кого урон.</param>
	/// <param name="damage">Сколько урона.</param>
	/// <param name="slow">Коэффициент замедления передвижения.</param>
	/// <param name="attackSlow">Коэффициент замедления атаки.</param>
	/// <param name="duration">Продолжительность действия замедлений в секундах.</param>
	/// <returns></returns>
	Unit.Profit TakeDamage(Unit unit, float damage, float slow, float attackSlow, float duration);
}
