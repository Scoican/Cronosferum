using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void SwitchSceneToStartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void SwitchSceneToMainMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
