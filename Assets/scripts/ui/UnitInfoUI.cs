using UnityEngine;
using UnityEngine.UI;

namespace Progress {
	/// <summary>
	/// Отображение информации о юните/казарме.
	/// </summary>
	public class UnitInfoUI : MonoBehaviour {

		// Фон.
		public GameObject background;
		// Кнопка апгрейда казармы.
		public GameObject upgradeButton;

		// Текстовый объект.
		protected Text text;
		// То, что сейчас выделено.
		protected Selectable thing;
		// Выделено сейчас или выделение уже нужно убрать.
		protected bool isSelected;

		// Цвет кнопки апгрейда казармы.
		private Color buttonColor;

		void Start() {
			text = GetComponent<Text>();
			text.enabled = false;

			background.SetActive(false);
			if (upgradeButton != null) {
				upgradeButton.SetActive(false);
				buttonColor = upgradeButton.GetComponent<Image>().color;
			}

			SpawnersManager.Instance.onUnitSelected -= OnUnitSelected;
			SpawnersManager.Instance.onUnitSelected += OnUnitSelected;
		}

		/// <summary>
		/// Если объект выделен, то включить фон, показать текст и кнопку, если казарма.
		/// Если нет - убрать выделение.
		/// </summary>
		protected virtual void OnUnitSelected(Selectable thing) {
			if (this.thing == thing && isSelected == thing.IsSelected()
				|| this.thing != thing && !thing.IsSelected()) {
				return;
			}
			this.thing = thing;
			isSelected = thing.IsSelected();
			text.enabled = isSelected;
			background.SetActive(isSelected);
			if (isSelected) {
				SetText();
			}
			if (upgradeButton != null) {
				upgradeButton.SetActive(thing is Spawner && isSelected);
			}
		}

		/// <summary>
		/// Установить текст в интерфейсе.
		/// </summary>
		protected virtual void SetText() {
			var txt = "";
			if (thing is Unit) {
				var unit = thing as Unit;
				txt = string.Format("Тип: {0}\nУровень: {1}\nЗдоровье: {2}\nСкорость: {3}\nАтака: {4}\nСкорость атаки: {5}\nЗона атаки: {6}\nЗащита: {7}",
					unit.PrettyType(),
					unit.Level + 1,
					unit.Health(),
					unit.Settings.Speed,
					unit.Settings.Attack,
					unit.Settings.AttackSpeed,
					unit.Settings.AttackRange,
					unit.Settings.Armor
				);
			} else if (thing is Spawner) {
				var spawner = thing as Spawner;
				txt = string.Format("Тип: {0}\nУровень: {1}\nСкорость тренировки: {2}\nСтоимость улучшения: {3}",
					spawner.PrettyType(),
					spawner.Level + 1,
					spawner.trainingSpeed,
					spawner.settings.Gold
				);
			}
			text.text = txt;
		}

		private void Update() {
			if (!isSelected || thing == null) {
				return;
			}

			SetText();

			if (upgradeButton != null && upgradeButton.activeSelf) {
				var color = EnoughGoldToUpgrade() ? buttonColor : Color.gray;
				if (color != upgradeButton.GetComponent<Image>().color) {
					upgradeButton.GetComponent<Image>().color = color;
				}
			}
		}

		/// <summary>
		/// Достаточно ли золота для апгрейда казармы.
		/// </summary>
		/// <returns></returns>
		private bool EnoughGoldToUpgrade() {
			var spawner = thing as Spawner;
			return spawner != null && spawner.settings.Gold <= Player.GoldAmount;
		}

		private void OnDestroy() {
			SpawnersManager.Instance.onUnitSelected -= OnUnitSelected;
		}
	}
}
