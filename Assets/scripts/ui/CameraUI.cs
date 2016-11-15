using UnityEngine;
using Progress;

/// <summary>
/// UI камеры.
/// </summary>
public class CameraUI : MonoBehaviour {

	void Start() {
		// Подписка на конец игры.
		Divan.Instance.OnGameEnd -= OnGameEnd;
		Divan.Instance.OnGameEnd += OnGameEnd;
	}

	private void OnGameEnd(bool win) {
		gameObject.SetActive(false);
	}
}
