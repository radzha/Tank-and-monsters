using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Показыет текущее число миньонов.
/// </summary>
public class MignonesUI : MonoBehaviour {

	private Text text;

	protected void Awake(){
		text = GetComponent<Text>();
	}

	void Update () {
		text.text = "Миньонов: " + SpawnersManager.Instance.Mignons().Count;
	}

}
