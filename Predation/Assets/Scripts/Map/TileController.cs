using Predation.Entities;
using Predation.Managers;
using Predation.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Predation.Map
{
	public class TileController : MonoBehaviour
	{
		private EntityManager entityManager;
		private Tile tile;
		private Color tileColor;

		private void Update()
		{
			if (!tile.HasFood && tile.CanGrowFood)
			{
				SpawnPlant(true);
			}
		}

		void OnMouseDown()
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}
			if (entityManager.GetEntityToSpawn() != null)
			{
				if (!tile.Occupied && !tile.HasObstacle && tile.Type != Tile.TileType.Water)
				{
					SpawnEntity(entityManager.GetEntityToSpawn(), false);
				}
				else
				{
					Debug.Log("CAN'T PLACE HERE!");
				}
			}
			else
			{
				Debug.Log("PLEASE SELECT AN ANIMAL!");
			}

		}

		private void OnMouseOver()
		{
			if (Input.GetMouseButtonDown(1))
			{
				Debug.Log($"This tile is {tile.Type} and has a position of {tile.Position} and occupation of {tile.EntitiesCount} and is {tile.Occupied}");
			}
		}

		void OnMouseEnter()
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}
			GetComponentInChildren<Renderer>().material.SetColor("_Color", tileColor + tileColor / 2);
		}

		void OnMouseExit()
		{
			GetComponentInChildren<Renderer>().material.SetColor("_Color", tileColor);
		}

		public void SpawnPlant(bool shouldGrow)
		{
			var randomPositionX = Random.Range(GetComponent<Tile>().GetComponent<Collider>().bounds.min.x, GetComponent<Tile>().GetComponent<Collider>().bounds.max.x);
			var randomPositionZ = Random.Range(GetComponent<Tile>().GetComponent<Collider>().bounds.min.z, GetComponent<Tile>().GetComponent<Collider>().bounds.max.z);
			var plant = Instantiate(EntityFactory.Instance.getEntity(Species.Plant).entityPrefab, new Vector3(randomPositionX, GetComponent<Tile>().Height / 10, randomPositionZ), Quaternion.identity);
			plant.GetComponent<Entity>().position = GetComponent<Tile>().Position;
			plant.GetComponent<Plant>().ShouldGrow = shouldGrow;
			EntityManager.Instance.Register(plant.GetComponent<Entity>());
			GetComponent<Tile>().HasFood = true;
		}

		public GameObject SpawnEntity(EntityBlueprint entityBlueprint, bool isChild)
		{
			if (tile == null)
			{
				tile = GetComponent<Tile>();
			}
			tile.EntitiesCount++;
			return InitializeEntity(entityBlueprint, isChild);
		}

		private GameObject InitializeEntity(EntityBlueprint entityBlueprint, bool isChild)
		{
			var newEntity = Instantiate(entityBlueprint.entityPrefab, new Vector3(GetComponent<Tile>().Position.x, GetComponent<Tile>().Height / 10, GetComponent<Tile>().Position.y), Quaternion.identity);
			if (entityBlueprint.name != "Plant")
			{
				newEntity.gameObject.GetComponent<BaseAnimalController>().currentTile = tile;
				newEntity.GetComponent<Animal>().InitializeAnimal(isChild);
			}
			else
			{
				tile.HasFood = true;
				tile.CanGrowFood = true;
			}
			newEntity.GetComponent<Entity>().position = tile.Position;
			EntityManager.Instance.Register(newEntity.GetComponent<Entity>());
			return newEntity;
		}

		private void Start()
		{
			entityManager = EntityManager.Instance;
			tile = GetComponent<Tile>();
			tileColor = GetComponentInChildren<Renderer>().material.color;
		}
	}
}