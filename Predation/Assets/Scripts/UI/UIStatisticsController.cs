using Predation.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Predation.UI
{
	public class UIStatisticsController : MonoBehaviour
	{
		private StatisticsManager statisticsManager;

		public Button SpeciesRepartitionButton;
		public Button DeathCausesButton;
		public Button PopulationEvolutionButton;
		public Button MortalityRateButton;
		public Button SpeedEvolutionButton;
		public Button SensoryDistanceEvolutionButton;
		public Button DesirabilityEvolutionButton;
		public PieGraphController PieGraphController;
		public LinearGraphController LinearGraphController;
		public GameObject PieGraphEntitySelector;

		private void Start()
		{
			statisticsManager = StatisticsManager.Instance;
			SpeciesRepartitionButton.onClick.AddListener(delegate { DisplaySpeciesStatistics(); });
			DeathCausesButton.onClick.AddListener(delegate { DisplayDeathCausesStatistics(); });
			PopulationEvolutionButton.onClick.AddListener(delegate { DisplayPopulationEvolutionStatistics(); });
			MortalityRateButton.onClick.AddListener(delegate { DisplayMortalityRateStatistics(); });
			SpeedEvolutionButton.onClick.AddListener(delegate { DisplaySpeedEvolutionStatistics(); });
			SensoryDistanceEvolutionButton.onClick.AddListener(delegate { DisplaySensoryDistanceStatistics(); });
			DesirabilityEvolutionButton.onClick.AddListener(delegate { DisplayDesirabilityStatistics(); });
		}

		private void DisplayDeathCausesStatistics()
		{
			LinearGraphController.gameObject.SetActive(false);
			PieGraphController.gameObject.SetActive(true);
			PieGraphEntitySelector.SetActive(false);
			PieGraphController.InitializePieChart(statisticsManager.GetDeathsStatistics());
		}

		private void DisplaySpeciesStatistics()
		{
			LinearGraphController.gameObject.SetActive(false);
			PieGraphController.ResetPieChart();
			PieGraphController.gameObject.SetActive(true);
			PieGraphEntitySelector.SetActive(true);
			PieGraphController.InitializePieChart(statisticsManager.GetCurrentSpeciesStatistics());
		}

		private void DisplayMortalityRateStatistics()
		{
			PieGraphController.gameObject.SetActive(false);
			LinearGraphController.ResetLinearGraph();
			LinearGraphController.gameObject.SetActive(true);
			LinearGraphController.InitializeLinearChart(statisticsManager.GetMortalityRateStatistics());
		}

		private void DisplayPopulationEvolutionStatistics()
		{
			PieGraphController.gameObject.SetActive(false);
			LinearGraphController.ResetLinearGraph();
			LinearGraphController.gameObject.SetActive(true);
			LinearGraphController.InitializeLinearChart(statisticsManager.GetPopulationEvolutionStatistics());
		}

		private void DisplaySpeedEvolutionStatistics()
		{
			PieGraphController.gameObject.SetActive(false);
			LinearGraphController.ResetLinearGraph();
			LinearGraphController.gameObject.SetActive(true);
			LinearGraphController.InitializeLinearChart(statisticsManager.GetSpeedEvolutionData());
		}

		private void DisplaySensoryDistanceStatistics()
		{
			PieGraphController.gameObject.SetActive(false);
			LinearGraphController.ResetLinearGraph();
			LinearGraphController.gameObject.SetActive(true);
			LinearGraphController.InitializeLinearChart(statisticsManager.GetSensoryDistanceEvolutionData());
		}

		private void DisplayDesirabilityStatistics()
		{
			PieGraphController.gameObject.SetActive(false);
			LinearGraphController.ResetLinearGraph();
			LinearGraphController.gameObject.SetActive(true);
			LinearGraphController.InitializeLinearChart(statisticsManager.GetDesirabilityEvolutionData());
		}
	}
}