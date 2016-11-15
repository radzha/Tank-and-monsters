using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Показыет текущее число врагов.
/// </summary>
public class EnemiesUI : MonoBehaviour {

	private Text text;

	protected void Awake(){
		text = GetComponent<Text>();
	}

	void Update () {
		text.text = "Врагов: " + SpawnersManager.Instance.EnemiesCount();
	}

}
