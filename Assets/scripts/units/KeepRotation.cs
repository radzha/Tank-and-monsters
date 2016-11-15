using UnityEngine;

/// <summary>
/// Сохраняет изначальую ориентацию UI здоровья юнита при поворотах последнего.
/// </summary>
public class KeepRotation : MonoBehaviour {
	private Quaternion rotation;

	private void Start () {
		rotation = transform.rotation;
	}

	private void Update () {
		transform.rotation = rotation;
	}
}
