using UnityEngine;
using Progress;
using System.Collections;

/// <summary>
/// Класс лучника.
/// </summary>
public class Archer : Unit {
	// Линия, демонстирующая выстрел.
	protected LineRenderer line;

	protected override void Awake() {
		base.Awake();
		line = transform.Find("Base/Gun").GetComponent<LineRenderer>();
	}

	/// <summary>
	/// Выстрел лучника.
	/// </summary>
	protected override void Fire() {
		StartCoroutine(FireForPeriod(LevelEditor.Instance.ArcherFirePeriod));
	}

	/// <summary>
	/// Показывает визуалку выстрела некоторое время после логического выстрела.
	/// </summary>
	/// <param name="period"></param>
	/// <returns></returns>
	private IEnumerator FireForPeriod(float period) {
		line.enabled = true;
		while (period > 0f) {
			if (target == null || target.aim as MonoBehaviour == null || (target.aim as MonoBehaviour).gameObject == null) {
				line.enabled = false;
				yield break;
			}
			var aim = ((MonoBehaviour) target.aim).gameObject;
			line.SetPosition(0, transform.position);
			line.SetPosition(1, aim.transform.position);
			period -= Time.deltaTime;
			yield return null;
		}
		line.enabled = false;
	}

}