/// <summary>
/// Объекты игры, доступные для выделения левой кнопкой мыши.
/// </summary>
public interface Selectable {
	/// <summary>
	/// Выделить объект.
	/// </summary>
	/// <param name="selected">Выделить, если true, убрать выделение, если false.</param>
	void SetSelected(bool selected);

	/// <summary>
	/// Выделен ли объект.
	/// </summary>
	bool IsSelected();
}
