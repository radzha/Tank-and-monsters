using UnityEngine;
using UnityEngine.UI;
using Progress;

public class GameEndUI : MonoBehaviour {

	// Затеняющий тонер.
	private Image toner;
	// Текст выигрыша/поражения.
	private Text text;

	void Start() {
		toner = GetComponent<Image>();
		text = GetComponentInChildren<Text>();

		text.enabled = false;
		toner.enabled = false;

		Divan.Instance.OnGameEnd -= OnGameEnd;
		Divan.Instance.OnGameEnd += OnGameEnd;
	}

	/// <summary>
	/// Действие в момент конца игры.
	/// </summary>
	/// <param name="win">Выиграл ли игрок.</param>
	private void OnGameEnd(bool win) {
		toner.enabled = true;
		text.enabled = true;
		text.text = win ? "Вы выиграли!" : "Вы проиграли!";
	}

	private void OnDestroy() {
		Divan.Instance.OnGameEnd -= OnGameEnd;
	}
	
}
