using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Обновляет текст UI режима камеры.
/// </summary>
public class UpdateText : MonoBehaviour {

	private Text text;

	void Start() {
		text = GetComponent<Text>();
		OnCameraSwitch();
		CameraManager.Instance.onSwitch -= OnCameraSwitch;
		CameraManager.Instance.onSwitch += OnCameraSwitch;
	}

	private void OnCameraSwitch() {
		if (text == null) {
			return;
		}
		text.text = CameraManager.Instance.AutoMove ? "Режим слежения" : "Свободный режим";
	}
}
