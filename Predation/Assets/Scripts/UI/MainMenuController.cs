using UnityEngine;

namespace Predation.UI
{
	public class MainMenuController : MonoBehaviour
	{
		private MainMenuView view;

		private void Start()
		{
			view = GetComponent<MainMenuView>();
			InitViewElements();
		}

		private void InitViewElements()
		{
			view.ExitGameButton.onClick.AddListener(ExitGame);
			view.StartNewGameButton.onClick.AddListener(OpenSetttingsPanel);
		}

		private void OpenSetttingsPanel()
		{
			view.SettingsPanel.SetActive(true);
		}

		public void ExitGame()
		{
			Application.Quit();
		}
	}
}