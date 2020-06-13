using AwesomeCharts;
using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Predation.UI
{
	public class PieGraphController : MonoBehaviour
	{
		[Header("Option Buttons")]
		public Button RabbitButton;
		public Button WolfButton;
		public Button PlantButton;

		public PieChart PieChart;
		public LegendView LegendView;

		public Color wolvesColor;
		public Color rabbitsColor;
		public Color plantsColor;

		public Color HungerColor;
		public Color ThirstColor;
		public Color AgeColor;
		public Color EatenColor;

		public TMP_Text NoDataWarning;

		private Color selectedButtonColor = new Color(1, 1, 1, 1);
		private Color unSelectedButtonColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

		private bool RabbitSelected = true;
		private bool WolfSelected = true;
		private bool PlantSelected = true;

		private PieEntry wolvesSet;
		private PieEntry rabbitsSet;
		private PieEntry plantsSet;

		private PieEntry hungerSet;
		private PieEntry thirstSet;
		private PieEntry ageSet;
		private PieEntry eatenSet;

		private void Awake()
		{
			InitializeUISets();
			InitializeButtons();
		}

		private void InitializeButtons()
		{
			WolfButton.onClick.AddListener(delegate { SetWolvesSet(); });
			RabbitButton.onClick.AddListener(delegate { SetRabbitsSet(); });
			PlantButton.onClick.AddListener(delegate { SetPlantsSet(); });
		}

		private void SetWolvesSet()
		{
			if (WolfSelected)
			{
				PieChart.GetChartData().DataSet.Entries.Remove(wolvesSet);
				PieChart.SetDirty();
				WolfSelected = false;
				WolfButton.GetComponent<Image>().color = unSelectedButtonColor;
			}
			else
			{
				if (wolvesSet.Value != 0)
				{
					PieChart.GetChartData().DataSet.Entries.Add(wolvesSet);
					PieChart.SetDirty();
				}
				WolfSelected = true;
				WolfButton.GetComponent<Image>().color = selectedButtonColor;
			}
		}

		private void SetRabbitsSet()
		{
			if (RabbitSelected)
			{
				PieChart.GetChartData().DataSet.Entries.Remove(rabbitsSet);
				PieChart.SetDirty();
				RabbitSelected = false;
				RabbitButton.GetComponent<Image>().color = unSelectedButtonColor;
			}
			else
			{
				if (rabbitsSet.Value != 0)
				{
					PieChart.GetChartData().DataSet.Entries.Add(rabbitsSet);
					PieChart.SetDirty();
				}
				RabbitSelected = true;
				RabbitButton.GetComponent<Image>().color = selectedButtonColor;
			}
		}

		private void SetPlantsSet()
		{
			if (PlantSelected)
			{
				PieChart.GetChartData().DataSet.Entries.Remove(plantsSet);
				PieChart.SetDirty();
				PlantSelected = false;
				PlantButton.GetComponent<Image>().color = unSelectedButtonColor;
			}
			else
			{
				if (plantsSet.Value != 0)
				{
					PieChart.GetChartData().DataSet.Entries.Add(plantsSet);
					PieChart.SetDirty();
				}
				PlantSelected = true;
				PlantButton.GetComponent<Image>().color = selectedButtonColor;
			}
		}

		private void InitializeUISets()
		{
			wolvesSet = new PieEntry();
			rabbitsSet = new PieEntry();
			plantsSet = new PieEntry();

			hungerSet = new PieEntry();
			thirstSet = new PieEntry();
			ageSet = new PieEntry();
			eatenSet = new PieEntry();


			wolvesSet.Color = wolvesColor;
			wolvesSet.Label = Constants.WOLVES;

			rabbitsSet.Color = rabbitsColor;
			rabbitsSet.Label = Constants.RABBITS;

			plantsSet.Color = plantsColor;
			plantsSet.Label = Constants.PLANTS;

			hungerSet.Color = HungerColor;
			hungerSet.Label = "Hunger";

			thirstSet.Color = ThirstColor;
			thirstSet.Label = "Thirst";

			ageSet.Color = AgeColor;
			ageSet.Label = "Age";

			eatenSet.Color = EatenColor;
			eatenSet.Label = "Eaten";
		}

		public void InitializePieChart(List<(float, string)> values)
		{
			if (IsDataEmpty(values))
			{
				NoDataWarning.gameObject.SetActive(true);
				PieChart.gameObject.SetActive(false);
				LegendView.gameObject.SetActive(false);
			}
			else
			{
				NoDataWarning.gameObject.SetActive(false);
				PieChart.gameObject.SetActive(true);
				LegendView.gameObject.SetActive(true);
				PieDataSet set = new PieDataSet();
				foreach (var value in values)
				{
					switch (value.Item2)
					{
						case "Wolf":
							wolvesSet.Value = value.Item1;
							set.AddEntry(wolvesSet);
							break;
						case "Rabbit":
							rabbitsSet.Value = value.Item1;
							set.AddEntry(rabbitsSet);
							break;
						case "Plant":
							plantsSet.Value = value.Item1;
							set.AddEntry(plantsSet);
							break;
						case "Hunger":
							hungerSet.Value = value.Item1;
							set.AddEntry(hungerSet);
							break;
						case "Thirst":
							thirstSet.Value = value.Item1;
							set.AddEntry(thirstSet);
							break;
						case "Age":
							ageSet.Value = value.Item1;
							set.AddEntry(ageSet);
							break;
						case "Eaten":
							eatenSet.Value = value.Item1;
							set.AddEntry(eatenSet);
							break;
					}
				}
				PieChart.GetChartData().DataSet = set;
				PieChart.SetDirty();
			}
		}

		public void ResetPieChart()
		{
			RabbitSelected = true;
			RabbitButton.GetComponent<Image>().color = selectedButtonColor;
			PlantSelected = true;
			PlantButton.GetComponent<Image>().color = selectedButtonColor;
			WolfSelected = true;
			WolfButton.GetComponent<Image>().color = selectedButtonColor;
			PieChart.GetChartData().Clear();
		}

		private bool IsDataEmpty(List<(float, string)> values)
		{
			foreach (var value in values)
			{
				if (value.Item1 != 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}
