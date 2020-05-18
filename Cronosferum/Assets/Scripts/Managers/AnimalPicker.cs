using Predation.Utils;
using UnityEngine;

namespace Predation
{
	public class AnimalPicker : MonoBehaviour
	{
		EntityFactory entityFactory;

		private void Start()
		{
			entityFactory = EntityFactory.Instance;
		}

		public void SelectRabbit()
		{
			EntityManager.Instance.SelectEntityToSpawn(entityFactory.getEntity(Constants.RABBIT_BLUEPRINT));
		}
		public void SelectChicken()
		{
			EntityManager.Instance.SelectEntityToSpawn(entityFactory.getEntity(Constants.CHICKEN_BLUEPRINT));
		}
		public void SelectWolf()
		{
			EntityManager.Instance.SelectEntityToSpawn(entityFactory.getEntity(Constants.WOLF_BLUEPRINT));
		}
	}

}
