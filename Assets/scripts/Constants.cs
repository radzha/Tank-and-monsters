/// <summary>
/// Класс глобальных констант.
/// </summary>
public class Constants {
	// Маска слоя юнитов.
	public const int UNIT_LAYER = 1 << 8;

	// Маска слоя земли.
	public const int FLOOR_LAYER = 1 << 9;

	// Допустимая погрешнеость. Используется для остановки движения.
	public const float Epsilon = 0.1f;
}
