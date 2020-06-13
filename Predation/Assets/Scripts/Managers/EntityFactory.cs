using Predation.Utils;
using UnityEngine;

namespace Predation.Entities
{
	public class EntityFactory : MonoBehaviour
	{
		public EntityBlueprint RabbitBlueprint;
		public EntityBlueprint ChickenBlueprint;
		public EntityBlueprint WolfBlueprint;
		public EntityBlueprint PlantBlueprint;

		private static EntityFactory instance;

		public static EntityFactory Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<EntityFactory>();
					if (instance == null)
					{
						var container = new GameObject("EntityFactory");
						instance = container.AddComponent<EntityFactory>();
					}
				}
				return instance;
			}
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Debug.LogError("More than one EntityFactory in the scene!");
				Destroy(gameObject);
			}
		}

		public EntityBlueprint getEntity(Species entityType)
		{
			switch (entityType)
			{
				case Species.Wolf:
					return WolfBlueprint;
				case Species.Chicken:
					return ChickenBlueprint;
				case Species.Rabbit:
					return RabbitBlueprint;
				case Species.Plant:
					return PlantBlueprint;
				default:
					return null;
			}
		}
	}
}

