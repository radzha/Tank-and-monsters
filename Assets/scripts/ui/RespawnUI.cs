using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Информацию о времени до воскрешения героя, его золоте и опыте.
/// </summary>
public class RespawnUI : MonoBehaviour {

	private Text text;

	protected void Awake() {
		text = GetComponent<Text>();
	}

	void Update() {
		var respawn = Progress.Fountain.Instance.gameObject.GetComponent<Spawner>().RespawnTime();
		text.text = string.Format("Золото: {0}\nОпыт: {1}\nВремя до воскрешения: {2}\n",
		                          Progress.Player.GoldAmount,
		                          Progress.Player.Experience,
		                          respawn > 0 ? respawn.ToString() : "-");
	}

}
