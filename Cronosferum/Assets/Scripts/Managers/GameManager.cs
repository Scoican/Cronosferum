using Predation.Utils;
using UnityEngine;

namespace Predation
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
		private UIManager UIManager;
		private EnviromentManager enviromentManager;
		private MapManager mapManager;
		private EntityManager entityManager;
		private InputManager inputManager;

		public static GameStates gameState;

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
			mapManager = MapManager.Instance;
			entityManager = EntityManager.Instance;
			enviromentManager = EnviromentManager.Instance;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.G))
			{
				EntityManager.Instance.GeneratePrey();
				EntityManager.Instance.GeneratePredators();
			}
		}
	}
}

