using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Predation.UI
{
	public class SettingsMenuView : MonoBehaviour
	{
		public TMP_Text MapSizeSliderText;
		public TMP_Text PreySliderText;
		public TMP_Text PredatorSliderText;
		public TMP_Text FoodSliderText;
		public TMP_Text OtherElementsSliderText;

		public Slider MapSizeSlider;
		public Slider PreySlider;
		public Slider PredatorSlider;
		public Slider FoodSlider;
		public Slider OtherElementsSlider;

		public Button GenerateButton;
		public Button CancelButton;

		public GameObject SettingsPanel;
	}
}