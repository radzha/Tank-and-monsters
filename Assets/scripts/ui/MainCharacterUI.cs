using Progress;

/// <summary>
/// Отображение специфической информации о главном персонаже.
/// </summary>
public class MainCharacterUI : UnitInfoUI {

	protected override void OnUnitSelected(Selectable unit) {
		var unit1 = unit as Unit;
		if (unit1 != null && unit1.IsPlayer) {
			base.OnUnitSelected(unit1);
		}
	}

	/// <summary>
	/// Установить текст в интерфейсе.
	/// </summary>
	protected override void SetText() {
		var txt = string.Format("Уровень: {0}\nОпыт: {1}\nЗолото: {2}\nМетеор. дождь: {3}\nЛедяная стрела: {4}",
			((MainCharacter)thing).Level + 1,
			Player.Experience,
			Player.GoldAmount,
			((MainCharacter)thing).MeteoRainTimerString,
			((MainCharacter)thing).IceArrowTimerString);
		text.text = txt;
	}
}
