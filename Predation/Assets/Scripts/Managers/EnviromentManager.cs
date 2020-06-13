using UnityEngine;

namespace Predation.Managers
{
	public class EnviromentManager : MonoBehaviour
	{
		private MapManager mapManager;
		private EntityManager entityManager;

		#region Singleton Instance
		private static EnviromentManager instance;

		public static EnviromentManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<EnviromentManager>();
					if (instance == null)
					{
						var container = new GameObject("EnviromentManager");
						instance = container.AddComponent<EnviromentManager>();
					}
				}
				return instance;
			}
		}
		#endregion

		private void Awake()
		{
			mapManager = MapManager.Instance;
			entityManager = EntityManager.Instance;
		}

		private void Start()
		{
			mapManager.InitializeMap();
			entityManager.InitializePopulation();
		}
	}
}

