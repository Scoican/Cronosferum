using UnityEngine;

namespace Predation
{
	public class EntityFactory : MonoBehaviour
	{
		private const string WOLF_BLUEPRINT = "wolf_blueprint";
		private const string CHICKEN_BLUEPRINT = "chicken_blueprint";
		private const string RABBIT_BLUEPRINT = "rabbot_blueprint";
		private const string PLANT_BLUEPRINT = "plant_blueprint";

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

		public EntityBlueprint getEntity(string entityType)
		{
			switch (entityType)
			{
				case WOLF_BLUEPRINT:
					return WolfBlueprint;
				case CHICKEN_BLUEPRINT:
					return ChickenBlueprint;
				case RABBIT_BLUEPRINT:
					return RabbitBlueprint;
				case PLANT_BLUEPRINT:
					return PlantBlueprint;
				default:
					return null;
			}
		}
	}
}

