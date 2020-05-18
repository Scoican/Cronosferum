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

		public StatisticsManager StatisticsManager;
		public UIManager UIManager;
		public EnviromentManager EnviromentManager;
		public MapManager MapManager;
		public EntityManager EntityManager;
		public InputManager InputManager;

		public static GameStates gameState;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Debug.LogError("More than one GameManager in the scene!");
				Destroy(gameObject);
			}

			StatisticsManager = new StatisticsManager();
			UIManager = new UIManager();
			EnviromentManager = new EnviromentManager();
			MapManager = new MapManager();
			EntityManager = new EntityManager();
			InputManager = new InputManager();
		}

		private void Update()
		{
			InputManager.Update();
		}
	}
}

