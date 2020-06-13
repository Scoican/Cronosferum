using Predation.Utils;
using UnityEngine;

namespace Predation.Managers
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager gameManagerInstance;

		public static GameManager Instance
		{
			get
			{
				if (gameManagerInstance == null)
				{
					gameManagerInstance = FindObjectOfType<GameManager>();
					if (gameManagerInstance == null)
					{
						var container = new GameObject("GameManager");
						gameManagerInstance = container.AddComponent<GameManager>();
					}
				}
				return gameManagerInstance;
			}
		}

		private StatisticsManager statisticsManager;
		private UIManager uIManager;
		private EnviromentManager enviromentManager;
		private MapManager mapManager;
		private EntityManager entityManager;

		public static GameStates gameState;
		public static float GameTimeSpeed;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Debug.LogError("More than one GameManager in the scene!");
				Destroy(gameObject);
			}
		}

		private void Start()
		{
			enviromentManager = EnviromentManager.Instance;
			mapManager = MapManager.Instance;
			entityManager = EntityManager.Instance;
			uIManager = UIManager.Instance;
			statisticsManager = StatisticsManager.Instance;
			uIManager.InGameMenuPanel.SetActive(true);
			uIManager.StatisticsMenuPanel.SetActive(false);
			uIManager.ExitWarningPanel.SetActive(false);
			GameTimeSpeed = 1f;
			gameState = GameStates.Running;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.G))
			{
				EntityManager.Instance.GeneratePrey();
				EntityManager.Instance.GeneratePredators();
			}
			if (Input.GetMouseButtonDown(1))
			{
				uIManager.CloseAnimalInformation();
			}
		}
	}
}

