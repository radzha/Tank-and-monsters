using Progress;
using UnityEngine;

/// <summary>
/// Управление камерой.
/// </summary>
public class CameraManager : Singleton<CameraManager> {
	// "Палка", на которой держится камера. Определяет фиксированное расстояние до героя.
	public Vector3 rod;
	// Сглаживание автоматического перемещения камеры.
	public float smoothness = 1f;
	// Сглаживание перемещения камеры за мышью.
	public float mouseSmoothness = 0.1f;
	// Доля от половины экрана. Отсчитывается от края. Если мышь заходит в эту область, начинается перемещение камеры.
	public float edgeCoeff = 0.2f;
	// Задержка между сменой режима управления камерой.
	public float switchCameraDelay = 0.2f;

	// Включен ли режим автоматического слежения камерой за игроком.
	public bool AutoMove {
		get {
			return _autoMove;
		}
		set {
			_autoMove = value;
			onSwitch();
		}
	}

	// Вызывается при смене режима камеры.
	public delegate void OnSwitch();
	public event OnSwitch onSwitch = delegate { };

	// Центр экрана.
	private Vector3 screenCenter;
	// Таймер задержки переключения камеры.
	private float switchTimer;
	// Игрок, за которым следит камера.
	private GameObject player;
	private bool _autoMove;

	void Start() {
		screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
	}

	void FixedUpdate() {
		if (Divan.gameStop) {
			return;
		}

		// Смена режима камеры.
		if (switchTimer > 0f) {
			switchTimer -= Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.C) && switchTimer <= 0f) {
			SwitchCameraMode();
		}

		if (AutoMove) {
			if (player == null) {
				var hero = SpawnersManager.Instance.MainCharacter();
				if (hero != null) {
					player = hero.gameObject;
				}
			}
			if (player != null) {
				var targetPosition = player.transform.position + rod;
				transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
			}
		} else {
			var mouseShift = Input.mousePosition - screenCenter;
			var normalShift = new Vector2(Mathf.Abs(mouseShift.x) / Screen.width * 2f, Mathf.Abs(mouseShift.y) / Screen.height * 2f);
			if (normalShift.x > 1 - edgeCoeff || normalShift.y > 1 - edgeCoeff) {
				var targetPosition = mouseShift + rod;
				transform.position = Vector3.Lerp(transform.position, targetPosition, mouseSmoothness * Time.deltaTime);
			}
		}
	}

	/// <summary>
	/// Меняет режим камеры.
	/// </summary>
	public void SwitchCameraMode() {
		AutoMove = !AutoMove;
		switchTimer = switchCameraDelay;
	}
}
