using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, контроллирующий маркер абилки метеоритного дождя.
/// </summary>
public class MeteoRainMarkerManager : Singleton<MeteoRainMarkerManager> {

	// Префаб.
	public GameObject meteoRainMarkerPrefab;

	[NonSerialized]
	// Изображение маркера.
	public Image meteoRainMarker;

	// Объект маркера.
	private GameObject marker;

	private RaycastHit hit;

	void Awake() {
		marker = Instantiate(meteoRainMarkerPrefab);
		meteoRainMarker = marker.GetComponent<Image>();
		marker.SetActive(false);
	}

	void Update() {
		if (!marker.activeSelf) {
			return;
		}

		// Перемещение маркера, если мышь направлена на игровое поле.
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hit, int.MaxValue, Constants.FLOOR_LAYER);

		if (hit.collider == null) {
			return;
		}

		marker.transform.position = new Vector3(hit.point.x, 0.06f, hit.point.z);
	}
}
