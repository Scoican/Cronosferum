using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenuController : MonoBehaviour
{
	private const string NONE_CHANCE = "NONE";
	private const string SMALL_CHANCE = "SMALL";
	private const string MEDIUM_CHANCE = "MEDIUM";
	private const string LARGE_CHANCE = "LARGE";
	private SettingsMenuView view;

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
				view.PredatorSlider.maxValue = 30;
				view.PreySlider.maxValue = 30;
				break;
			case 2:
				view.MapSizeSliderText.text = MEDIUM_CHANCE;
				view.PredatorSlider.maxValue = 40;
				view.PreySlider.maxValue = 40;
				break;
			case 3:
				view.MapSizeSliderText.text = LARGE_CHANCE;
				view.PredatorSlider.maxValue = 50;
				view.PreySlider.maxValue = 50;
				break;
		}
		GameSettings.MapSize= (int)view.MapSizeSlider.value;
		GameSettings.HasBeenModified = true;
	}

	private void OnPreyPopulationValueChange()
	{
		view.PreySliderText.text = view.PreySlider.value.ToString();
		GameSettings.PreyPopulation = (int)view.PreySlider.value;
		GameSettings.HasBeenModified = true;
	}

	private void OnPredatorPopulationValueChange()
	{
		view.PredatorSliderText.text = view.PredatorSlider.value.ToString();
		GameSettings.PredatorPopulation = (int)view.PredatorSlider.value;
		GameSettings.HasBeenModified = true;
	}

	private void OnFoodSpawnSliderChange()
	{
		switch (view.FoodSlider.value)
		{
			case 0:
				view.FoodSliderText.text = NONE_CHANCE;
				break;
			case 1:
				view.FoodSliderText.text = SMALL_CHANCE;
				break;
			case 2:
				view.FoodSliderText.text = MEDIUM_CHANCE;
				break;
			case 3:
				view.FoodSliderText.text = LARGE_CHANCE;
				break;
		}
		GameSettings.FoodPercentage = (int)view.FoodSlider.value;
		GameSettings.HasBeenModified = true;
	}

	private void OnOtherElemenetsSliderValueChange()
	{
		switch (view.OtherElementsSlider.value)
		{
			case 0:
				view.OtherElementsSliderText.text = NONE_CHANCE;
				break;
			case 1:
				view.OtherElementsSliderText.text = SMALL_CHANCE;
				break;
			case 2:
				view.OtherElementsSliderText.text = MEDIUM_CHANCE;
				break;
			case 3:
				view.OtherElementsSliderText.text = LARGE_CHANCE;
				break;
		}
		GameSettings.OtherElementsPercentage = (int)view.OtherElementsSlider.value;
		GameSettings.HasBeenModified = true;
	}

	private void StartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}


	private void CancelSettings()
	{
		view.SettingsPanel.SetActive(false);
	}
}
