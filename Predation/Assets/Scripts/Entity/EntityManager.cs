using Predation.Entities;
using Predation.Map;
using Predation.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Predation.Managers
{
	public class EntityManager : MonoBehaviour
	{
		#region Singleton Instance
		private static EntityManager instance;
		public static EntityManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<EntityManager>();
					if (instance == null)
					{
						var container = new GameObject("EntityManager");
						instance = container.AddComponent<EntityManager>();
					}
				}
				return instance;
			}
		}

		#endregion

		public EntityBlueprint entityBlueprint;
		public Dictionary<int, Entity> Entities { get => entities; private set => entities = value; }
		public Dictionary<CauseOfDeath, List<Entity>> Deaths { get => deaths; private set => deaths = value; }

		private Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
		private Dictionary<CauseOfDeath, List<Entity>> deaths = new Dictionary<CauseOfDeath, List<Entity>>();
		private static int currentID = 0;
		private MapManager mapManager;
		private bool IsInitialPopulationZero;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Debug.LogError("More than one EntityManager in the scene!");
				Destroy(gameObject);
			}
			foreach (CauseOfDeath death in Enum.GetValues(typeof(CauseOfDeath)))
			{
				deaths[death] = new List<Entity>();
			}
		}

		private void Start()
		{
			mapManager = MapManager.Instance;
			IsInitialPopulationZero = GameSettings.IsPopulationZero;
		}

		private void Update()
		{
			if (GameManager.gameState == GameStates.Running)
			{
				var rabbitCount = GetEntitiesBySpecies(Species.Rabbit).Count;
				var wolfCount = GetEntitiesBySpecies(Species.Wolf).Count;
				if (wolfCount >= 125 || rabbitCount >= 125)
				{
					UIManager.Instance.OpenEndGamePanel("Species overpopulation!");
					GameManager.gameState = GameStates.Finished;
				}
				else if (rabbitCount == 0 && wolfCount == 0 && !IsInitialPopulationZero)
				{
					UIManager.Instance.OpenEndGamePanel("All species are extinct!");
					GameManager.gameState = GameStates.Finished;
				}
				if(rabbitCount != 0 || wolfCount != 0)
				{
					IsInitialPopulationZero = false;
				}
			}
		}

		public void InitializePopulation()
		{
			GeneratePrey();
			GeneratePredators();
		}

		public void SelectEntityToSpawn(EntityBlueprint entityBlueprint)
		{
			this.entityBlueprint = entityBlueprint;
		}

		public EntityBlueprint GetEntityToSpawn()
		{
			return entityBlueprint;
		}

		public void Register(Entity entity)
		{
			if (entity == null)
				return;
			if (entity.Id != -1 && entities.ContainsKey(entity.Id))
				return;

			entity.Id = currentID;
			entities.Add(entity.Id, entity);
			currentID++;
		}

		public void Unregister(Entity entity)
		{
			if (entity == null)
				return;
			if (entities.ContainsKey(entity.Id))
				entities.Remove(entity.Id);
		}

		public void RegisterDeath(Entity entity, CauseOfDeath causeOfDeath)
		{
			deaths[causeOfDeath].Add(entity);
			Unregister(entity);
			mapManager.GetTile(entity.position).EntitiesCount--;
		}

		public void GeneratePrey()
		{
			var preyPopulationSize = GameSettings.PreyPopulation;
			var mapSize = GameSettings.MapSize - 1;
			var unocupiedTiles = mapManager.GetUnoccupiedTiles();

			if (unocupiedTiles.Count < preyPopulationSize)
			{
				preyPopulationSize = unocupiedTiles.Count / 2;
			}

			while (preyPopulationSize > 0)
			{

				var randomXMapPosition = UnityEngine.Random.Range(0, mapSize);
				var randomYMapPosition = UnityEngine.Random.Range(0, mapSize);
				if (unocupiedTiles.ContainsKey(new Position(randomXMapPosition, randomYMapPosition)))
				{
					var tile = mapManager.GetTile(new Position(randomXMapPosition, randomYMapPosition));
					preyPopulationSize--;
					tile.GetComponent<TileController>().SpawnEntity(EntityFactory.Instance.getEntity(Species.Rabbit), false);
					unocupiedTiles.Remove(tile.Position);
				}
			}
		}

		public void GeneratePredators()
		{
			var predatorPopulationSize = GameSettings.PredatorPopulation;
			var mapSize = GameSettings.MapSize - 1;
			var unocupiedTiles = mapManager.GetUnoccupiedTiles();

			if (unocupiedTiles.Count < predatorPopulationSize)
			{
				predatorPopulationSize = unocupiedTiles.Count / 2;
			}

			while (predatorPopulationSize > 0)
			{

				var randomXMapPosition = UnityEngine.Random.Range(0, mapSize);
				var randomYMapPosition = UnityEngine.Random.Range(0, mapSize);
				if (unocupiedTiles.ContainsKey(new Position(randomXMapPosition, randomYMapPosition)))
				{
					var tile = mapManager.GetTile(new Position(randomXMapPosition, randomYMapPosition));
					predatorPopulationSize--;
					tile.GetComponent<TileController>().SpawnEntity(EntityFactory.Instance.getEntity(Species.Wolf), false);
				}
			}
		}

		private List<Entity> GetEntitiesBySpecies(Species species)
		{
			var entitiesBySpecies = new List<Entity>();
			foreach (var entity in entities.Values)
			{
				if (entity.species == species)
				{
					entitiesBySpecies.Add(entity);
				}
			}
			return entitiesBySpecies;
		}
	}
}