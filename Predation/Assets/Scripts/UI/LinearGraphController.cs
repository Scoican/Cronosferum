using AwesomeCharts;
using Predation.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Predation.UI
{
	public class LinearGraphController : MonoBehaviour
	{
		[Header("Option Buttons")]
		public Button AllEntitiesButton;
		public Button RabbitButton;
		public Button WolfButton;

		public LineChart LineChart;
		public LegendView LegendView;

		public Texture GradientBackground;
		public Color allEntitiesColor;
		public Color wolvesColor;
		public Color rabbitsColor;

		public TMP_Text NoDataWarning;

		private Color selectedButtonColor = new Color(1, 1, 1, 1);
		private Color unSelectedButtonColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

		private bool allEntitiesSelected = true;
		private bool rabbitSelected = true;
		private bool wolfSelected = true;

		private LineDataSet allEntitiesSet;
		private LineDataSet wolvesSet;
		private LineDataSet rabbitsSet;

		private void Awake()
		{
			InitializeUISets();
			InitializeButtons();
		}

		private void InitializeButtons()
		{
			AllEntitiesButton.onClick.AddListener(delegate { SetAllEntitiesSet(); });
			WolfButton.onClick.AddListener(delegate { SetWolvesSet(); });
			RabbitButton.onClick.AddListener(delegate { SetRabbitsSet(); });
		}

		private void SetAllEntitiesSet()
		{
			if (allEntitiesSelected)
			{
				LineChart.GetChartData().DataSets.Remove(allEntitiesSet);
				LineChart.SetDirty();
				allEntitiesSelected = false;
				foreach (Transform child in AllEntitiesButton.transform)
				{
					child.GetComponent<Image>().color = unSelectedButtonColor;
				}
			}
			else
			{
				if (allEntitiesSet.GetEntriesCount() != 0)
				{
					LineChart.GetChartData().DataSets.Insert(0, allEntitiesSet);
					LineChart.SetDirty();
				}
				allEntitiesSelected = true;
				foreach (Transform child in AllEntitiesButton.transform)
				{
					child.GetComponent<Image>().color = selectedButtonColor;
				}
			}
		}

		private void SetWolvesSet()
		{
			if (wolfSelected)
			{
				LineChart.GetChartData().DataSets.Remove(wolvesSet);
				LineChart.SetDirty();
				wolfSelected = false;
				WolfButton.GetComponent<Image>().color = unSelectedButtonColor;
			}
			else
			{
				if (wolvesSet.GetEntriesCount() != 0)
				{
					LineChart.GetChartData().DataSets.Add(wolvesSet);
					LineChart.SetDirty();
				}
				wolfSelected = true;
				WolfButton.GetComponent<Image>().color = selectedButtonColor;
			}
		}

		private void SetRabbitsSet()
		{
			if (rabbitSelected)
			{
				LineChart.GetChartData().DataSets.Remove(rabbitsSet);
				LineChart.SetDirty();
				rabbitSelected = false;
				RabbitButton.GetComponent<Image>().color = unSelectedButtonColor;
			}
			else
			{
				if (rabbitsSet.GetEntriesCount() != 0)
				{
					LineChart.GetChartData().DataSets.Add(rabbitsSet);
					LineChart.SetDirty();
				}
				rabbitSelected = true;
				RabbitButton.GetComponent<Image>().color = selectedButtonColor;
			}
		}

		private void InitializeUISets()
		{
			allEntitiesSet = new LineDataSet();
			wolvesSet = new LineDataSet();
			rabbitsSet = new LineDataSet();

			allEntitiesSet.FillTexture = GradientBackground;
			allEntitiesSet.FillColor = allEntitiesColor;
			allEntitiesSet.LineColor = allEntitiesColor;
			allEntitiesSet.Title = Constants.ALL_ENTITIES;

			wolvesSet.FillTexture = GradientBackground;
			wolvesSet.FillColor = wolvesColor;
			wolvesSet.LineColor = wolvesColor;
			wolvesSet.Title = Constants.WOLVES;

			rabbitsSet.FillTexture = GradientBackground;
			rabbitsSet.FillColor = rabbitsColor;
			rabbitsSet.LineColor = rabbitsColor;
			rabbitsSet.Title = Constants.RABBITS;
		}

		public void InitializeLinearChart(Dictionary<string, List<float>> values)
		{
			if (IsDataEmpty(values))
			{
				NoDataWarning.gameObject.SetActive(true);
				LineChart.gameObject.SetActive(false);
				LegendView.gameObject.SetActive(false);
			}
			else
			{
				NoDataWarning.gameObject.SetActive(false);
				LineChart.gameObject.SetActive(true);
				LegendView.gameObject.SetActive(true);
				foreach (var key in values.Keys)
				{
					switch (key)
					{
						case "All entities":
							allEntitiesSet.Clear();
							for (var i = 0; i < values[key].Count; i++)
							{
								if (i == 0)
								{
									var initialEntry = new LineEntry(0, 0);
									allEntitiesSet.AddEntry(initialEntry);
									var lineEntry = new LineEntry(i + 0.1f, values[key][i]);
									allEntitiesSet.AddEntry(lineEntry);
								}
								else if (values[key][i] != values[key][i - 1])
								{
									var lineEntry = new LineEntry(i, values[key][i]);
									allEntitiesSet.AddEntry(lineEntry);
								}
							}
							break;
						case "Wolves":
							wolvesSet.Clear();
							for (var i = 0; i < values[key].Count; i++)
							{
								if (i == 0)
								{
									var initialEntry = new LineEntry(0, 0);
									wolvesSet.AddEntry(initialEntry);
									var lineEntry = new LineEntry(i + 0.1f, values[key][i]);
									wolvesSet.AddEntry(lineEntry);
								}
								else if (values[key][i] != values[key][i - 1])
								{
									var lineEntry = new LineEntry(i, values[key][i]);
									wolvesSet.AddEntry(lineEntry);
								}
							}
							break;
						case "Rabbits":
							rabbitsSet.Clear();
							for (var i = 0; i < values[key].Count; i++)
							{
								if (i == 0)
								{
									var initialEntry = new LineEntry(0, 0);
									rabbitsSet.AddEntry(initialEntry);
									var lineEntry = new LineEntry(i + 0.1f, values[key][i]);
									rabbitsSet.AddEntry(lineEntry);
								}
								else if (values[key][i] != values[key][i - 1])
								{
									var lineEntry = new LineEntry(i, values[key][i]);
									rabbitsSet.AddEntry(lineEntry);
								}
							}
							break;
					}
				}
				LineChart.GetChartData().Clear();
				LineChart.GetChartData().DataSets.Add(allEntitiesSet);
				LineChart.GetChartData().DataSets.Add(wolvesSet);
				LineChart.GetChartData().DataSets.Add(rabbitsSet);
				LineChart.SetDirty();
			}
		}

		public void ResetLinearGraph()
		{
			allEntitiesSelected = true;
			foreach (Transform child in AllEntitiesButton.transform)
			{
				child.GetComponent<Image>().color = selectedButtonColor;
			}
			rabbitSelected = true;
			RabbitButton.GetComponent<Image>().color = selectedButtonColor;
			wolfSelected = true;
			WolfButton.GetComponent<Image>().color = selectedButtonColor;
			LineChart.GetChartData().Clear();
		}

		private bool IsDataEmpty(Dictionary<string, List<float>> values)
		{
			foreach (var key in values.Keys)
			{
				foreach (var value in values[key])
				{
					if (value != 0)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
