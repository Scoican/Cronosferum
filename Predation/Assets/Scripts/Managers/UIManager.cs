using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Predation.Utils;
using Predation.UI;
using Predation.Entities;

namespace Predation.Managers
{
	public class UIManager : MonoBehaviour
	{
		#region Singleton Instance
		private static UIManager instance;
		public static UIManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<UIManager>();
					if (instance == null)
					{
						var container = new GameObject("UIManager");
						instance = container.AddComponent<UIManager>();
					}
				}
				return instance;
			}
		}
		#endregion

		[Header("UI Panels")]
		public GameObject InGameMenuPanel;
		public GameObject StatisticsMenuPanel;

		[Header("In Game UI Menu")]
		public AnimalPicker animalPicker;
		public Slider timeSlider;
		public Button OpenStatisticsButton;
		public Button ExitGameButton;
		public Button RestartGameButton;
		public Button PauseGameButton;
		public AnimalInformationController animalInformation;

		[Header("Statistics UI Menu")]
		public GameObject StatisticsScrollView;
		public GameObject StatisticsDisplayer;
		public Button CloseStatisticsButton;

		[Header("Exit Warning UI Menu")]
		public GameObject ExitWarningPanel;
		public Button AffirmativeAnswerButton;
		public Button NegativeAnswerButton;

		[Header("End Game UI Menu")]
		public GameObject EndGamePanel;
		public TMP_Text EndText;
		public Button RetryAnswerButton;
		public Button ExitAnswerButton;
		public Button StatisticsAnswerButton;
		public Button ContinueAnswerButton;

		[Header("Settings Menu Panel")]
		public SettingsMenuController SettingsMenu;

		private bool isContinued = false;
		private void Start()
		{
			OpenStatisticsButton.onClick.AddListener(delegate { OnOpenStatisticsButtonClicked(); });
			StatisticsAnswerButton.onClick.AddListener(delegate { OnOpenStatisticsButtonClicked(); });
			CloseStatisticsButton.onClick.AddListener(delegate { OnCloseStatisticsButtonClicked(); });
			ContinueAnswerButton.onClick.AddListener(delegate { ContinueGameplayEndGamePanel(); });
			ExitGameButton.onClick.AddListener(delegate { ExitGame(); });
			ExitAnswerButton.onClick.AddListener(delegate { GoToMainMenu(); });
			RestartGameButton.onClick.AddListener(delegate { RestartGame(); });
			AffirmativeAnswerButton.onClick.AddListener(delegate { GoToMainMenu(); });
			NegativeAnswerButton.onClick.AddListener(delegate { CancelExit(); });
			RetryAnswerButton.onClick.AddListener(delegate { RestartGame(); });
			PauseGameButton.onClick.AddListener(delegate { PauseGame(); });
		}

		private void ContinueGameplayEndGamePanel()
		{
			GameManager.gameState = GameStates.Continued;
			EndGamePanel.SetActive(false);
		}

		private void PauseGame()
		{
			if (GameManager.gameState == GameStates.Running || GameManager.gameState == GameStates.Continued)
			{
				GameManager.gameState = GameStates.Paused;
				PauseGameButton.image.color = new Color(0.5f, 0.5f, 0.5f, 1);
				isContinued = true;
			}
			else if (GameManager.gameState == GameStates.Paused)
			{
				GameManager.gameState = isContinued ? GameStates.Continued : GameStates.Running;
				PauseGameButton.image.color = new Color(1, 1, 1, 1);
			}
		}

		private void RestartGame()
		{
			EndGamePanel.SetActive(false);
			SettingsMenu.gameObject.SetActive(true);
		}

		private void CancelExit()
		{
			ExitWarningPanel.SetActive(false);
		}

		private void GoToMainMenu()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}

		private void ExitGame()
		{
			ExitWarningPanel.SetActive(true);
		}

		public void ChangeTimeScale()
		{
			if (timeSlider.value == 0)
			{
				GameManager.GameTimeSpeed = 0.001f;
			}
			else
			{
				GameManager.GameTimeSpeed = timeSlider.value;
			}
		}

		private void OnOpenStatisticsButtonClicked()
		{
			EndGamePanel.SetActive(false);
			InGameMenuPanel.SetActive(false);
			StatisticsMenuPanel.SetActive(true);
		}

		private void OnCloseStatisticsButtonClicked()
		{
			InGameMenuPanel.SetActive(true);
			StatisticsMenuPanel.SetActive(false);
		}

		public void DisplayAnimalInformation(Animal animal)
		{
			animalInformation.gameObject.SetActive(true);
			animalInformation.DisplayAnimalInformation(animal);
		}

		public void CloseAnimalInformation()
		{
			animalInformation.gameObject.SetActive(false);
		}

		public void OpenEndGamePanel(string endGameText)
		{
			EndText.text = endGameText;
			EndGamePanel.SetActive(true);
		}

		public void CloseEndGamePanel()
		{
			EndGamePanel.SetActive(false);
		}
	}
}