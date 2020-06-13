using Predation.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Predation.UI
{
	public class SettingsMenuController : MonoBehaviour
	{
		private const string NONE_CHANCE = "NONE";
		private const string SMALL_CHANCE = "SMALL";
		private const string MEDIUM_CHANCE = "MEDIUM";
		private const string LARGE_CHANCE = "LARGE";
		private SettingsMenuView view;
		private float foodValue = 0f;
		private float obstacleValue = 0f;

		void Start()
		{
			view = GetComponent<SettingsMenuView>();
			InitViewElements();
		}

		private void InitViewElements()
		{
			view.MapSizeSlider.onValueChanged.AddListener(delegate { OnMapSizeSliderValueChange(); });
			view.PreySlider.onValueChanged.AddListener(delegate { OnPreyPopulationValueChange(); });
			view.PredatorSlider.onValueChanged.AddListener(delegate { OnPredatorPopulationValueChange(); });
			view.FoodSlider.onValueChanged.AddListener(delegate { OnFoodSpawnSliderChange(); });
			view.OtherElementsSlider.onValueChanged.AddListener(delegate { OnOtherElemenetsSliderValueChange(); });
			view.GenerateButton.onClick.AddListener(StartGame);
			view.CancelButton.onClick.AddListener(CancelSettings);
		}

		private void OnMapSizeSliderValueChange()
		{
			switch (view.MapSizeSlider.value)
			{
				case 1:
					view.MapSizeSliderText.text = SMALL_CHANCE;
					view.PredatorSlider.maxValue = 15;
					view.PreySlider.maxValue = 50;
					break;
				case 2:
					view.MapSizeSliderText.text = MEDIUM_CHANCE;
					view.PredatorSlider.maxValue = 25;
					view.PreySlider.maxValue = 75;
					break;
				case 3:
					view.MapSizeSliderText.text = LARGE_CHANCE;
					view.PredatorSlider.maxValue = 25;
					view.PreySlider.maxValue = 75;
					break;
			}
		}

		private void OnPreyPopulationValueChange()
		{
			view.PreySliderText.text = view.PreySlider.value.ToString();
		}

		private void OnPredatorPopulationValueChange()
		{
			view.PredatorSliderText.text = view.PredatorSlider.value.ToString();
		}

		private void OnFoodSpawnSliderChange()
		{
			switch (view.FoodSlider.value)
			{
				case 0:
					view.FoodSliderText.text = NONE_CHANCE;
					foodValue = 0f;
					break;
				case 1:
					view.FoodSliderText.text = SMALL_CHANCE;
					foodValue = 0.05f;
					break;
				case 2:
					view.FoodSliderText.text = MEDIUM_CHANCE;
					foodValue = 0.15f;
					break;
				case 3:
					view.FoodSliderText.text = LARGE_CHANCE;
					foodValue = 0.30f;
					break;
			}
		}

		private void OnOtherElemenetsSliderValueChange()
		{
			switch (view.OtherElementsSlider.value)
			{
				case 0:
					view.OtherElementsSliderText.text = NONE_CHANCE;
					obstacleValue = 0f;
					break;
				case 1:
					view.OtherElementsSliderText.text = SMALL_CHANCE;
					obstacleValue = 0.05f;
					break;
				case 2:
					view.OtherElementsSliderText.text = MEDIUM_CHANCE;
					obstacleValue = 0.1f;
					break;
				case 3:
					view.OtherElementsSliderText.text = LARGE_CHANCE;
					obstacleValue = 0.15f;
					break;
			}
		}

		private void StartGame()
		{
			GameSettings.MapSize = (int)view.MapSizeSlider.value;
			GameSettings.PreyPopulation = (int)view.PreySlider.value;
			GameSettings.PredatorPopulation = (int)view.PredatorSlider.value;
			GameSettings.OtherElementsPercentage = obstacleValue;
			GameSettings.FoodPercentage = foodValue;
			SceneManager.LoadScene("GameScene");
		}


		private void CancelSettings()
		{
			view.SettingsPanel.SetActive(false);
		}
	}
}
