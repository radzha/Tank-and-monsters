using UnityEngine;

/// <summary>
/// Класс, строящий стены, ограничивающие поле боя.
/// </summary>
public class WallsInit : MonoBehaviour {

	public Transform wallPrefab;
	public Vector3 scale = new Vector3(5.5f, 1f, 2f);
	public Vector3 position = new Vector3(-17.92f, 9.24f, 18.48f);

	void Start() {
		var walls = new Transform[4];
		for (var i = 0; i < 4; i++) {
			walls[i] = Instantiate(wallPrefab);
			walls[i].SetParent(transform);
			walls[i].localScale = scale;
			walls[i].localPosition = position;
		}

		walls[1].localPosition = new Vector3(-position.x, position.y, -position.z);
		walls[2].localPosition = new Vector3(position.x, position.y, -position.z);
		walls[2].Rotate(new Vector3(0f, 0f, 90f));
		walls[3].localPosition = new Vector3(-position.x, position.y, position.z);
		walls[3].Rotate(new Vector3(0f, 0f, 90f));
	}
}
