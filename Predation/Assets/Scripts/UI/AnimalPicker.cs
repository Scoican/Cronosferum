using Predation.Entities;
using Predation.Managers;
using Predation.Utils;
using UnityEngine;

namespace Predation.UI
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
			EntityManager.Instance.SelectEntityToSpawn(entityFactory.getEntity(Species.Rabbit));
		}
		public void SelectChicken()
		{
			EntityManager.Instance.SelectEntityToSpawn(entityFactory.getEntity(Species.Chicken));
		}
		public void SelectWolf()
		{
			EntityManager.Instance.SelectEntityToSpawn(entityFactory.getEntity(Species.Wolf));
		}

		public void SelectPlant()
		{
			EntityManager.Instance.SelectEntityToSpawn(entityFactory.getEntity(Species.Plant));
		}
	}
}
