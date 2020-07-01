using UnityEngine;
using System.Collections.Generic;
using Predation.Utils;
using System;
using System.Collections;
using Predation.Entities;

namespace Predation.Managers
{
	public class StatisticsManager : MonoBehaviour
	{
		#region Singleton Instance
		private static StatisticsManager instance;
		public static StatisticsManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<StatisticsManager>();
					if (instance == null)
					{
						var container = new GameObject("StatisticsManager");
						instance = container.AddComponent<StatisticsManager>();
					}
				}
				return instance;
			}
		}
		#endregion

		private EntityManager entityManager;
		private Dictionary<string, List<float>> populationEvolutionData;
		private Dictionary<string, List<float>> mortalityRateData;
		private Dictionary<string, List<float>> speedEvolutionData;
		private Dictionary<string, List<float>> sensoryDistanceData;
		private Dictionary<string, List<float>> desirabilityData;

		private void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Debug.LogError("More than one StatisticsManager in the scene!");
				Destroy(gameObject);
			}
		}

		private void Start()
		{
			entityManager = EntityManager.Instance;
			InitializeDictionaries();
			StartCoroutine(UpdateLineData());
		}

		private void InitializeDictionaries()
		{
			populationEvolutionData = new Dictionary<string, List<float>>();
			populationEvolutionData.Add(Constants.ALL_ENTITIES, new List<float>());
			populationEvolutionData.Add(Constants.WOLVES, new List<float>());
			populationEvolutionData.Add(Constants.RABBITS, new List<float>());
			populationEvolutionData.Add(Constants.PLANTS, new List<float>());

			mortalityRateData = new Dictionary<string, List<float>>();
			mortalityRateData.Add(Constants.ALL_ENTITIES, new List<float>());
			mortalityRateData.Add(Constants.WOLVES, new List<float>());
			mortalityRateData.Add(Constants.RABBITS, new List<float>());
			mortalityRateData.Add(Constants.PLANTS, new List<float>());

			speedEvolutionData = new Dictionary<string, List<float>>();
			speedEvolutionData.Add(Constants.ALL_ENTITIES, new List<float>());
			speedEvolutionData.Add(Constants.WOLVES, new List<float>());
			speedEvolutionData.Add(Constants.RABBITS, new List<float>());

			sensoryDistanceData = new Dictionary<string, List<float>>();
			sensoryDistanceData.Add(Constants.ALL_ENTITIES, new List<float>());
			sensoryDistanceData.Add(Constants.WOLVES, new List<float>());
			sensoryDistanceData.Add(Constants.RABBITS, new List<float>());

			desirabilityData = new Dictionary<string, List<float>>();
			desirabilityData.Add(Constants.ALL_ENTITIES, new List<float>());
			desirabilityData.Add(Constants.WOLVES, new List<float>());
			desirabilityData.Add(Constants.RABBITS, new List<float>());
		}

		public List<(float, string)> GetCurrentSpeciesStatistics()
		{
			var values = new List<(float, string)>();
			foreach (Species species in Enum.GetValues(typeof(Species)))
			{
				var value = GetEntitiesBySpecies(species, entityManager.Entities).Count;
				if (value != 0)
				{
					values.Add((value, species.ToString()));
				}
			}
			return values;
		}

		public List<(float, string)> GetDeathsStatistics()
		{
			var values = new List<(float, string)>();
			foreach (CauseOfDeath causeOfDeath in Enum.GetValues(typeof(CauseOfDeath)))
			{
				var value = 0;
				foreach (var entity in entityManager.Deaths[causeOfDeath])
				{
					if (entity.species != Species.Plant)
					{
						value++;
					}
				}
				if (value != 0)
				{
					values.Add((value, causeOfDeath.ToString()));
				}
			}
			return values;
		}

		public Dictionary<string, List<float>> GetPopulationEvolutionStatistics()
		{
			return populationEvolutionData;
		}

		public Dictionary<string, List<float>> GetMortalityRateStatistics()
		{
			return mortalityRateData;
		}

		public Dictionary<string, List<float>> GetSpeedEvolutionData()
		{
			return speedEvolutionData;
		}

		public Dictionary<string, List<float>> GetSensoryDistanceEvolutionData()
		{
			return sensoryDistanceData;
		}

		public Dictionary<string, List<float>> GetDesirabilityEvolutionData()
		{
			return desirabilityData;
		}

		private IEnumerator UpdateLineData()
		{
			while (true)
			{
				ObtainLineData();
				yield return new WaitForSeconds(5 / GameManager.GameTimeSpeed);
			}
		}

		private void ObtainLineData()
		{
			ObtainPopulationEvolutionData();
			ObtainMortalityRateData();
			ObtainSpeedEvolutionData();
			ObtainSensoryDistanceEvolutionData();
			ObtainDesirabilityEvolutionData();
		}

		private void ObtainPopulationEvolutionData()
		{
			var wolvesValue = GetEntitiesBySpecies(Species.Wolf, entityManager.Entities).Count;
			var rabbitsValue = GetEntitiesBySpecies(Species.Rabbit, entityManager.Entities).Count;
			var allEntitiesValue = wolvesValue + rabbitsValue;
			populationEvolutionData[Constants.ALL_ENTITIES].Add(allEntitiesValue);
			populationEvolutionData[Constants.WOLVES].Add(wolvesValue);
			populationEvolutionData[Constants.RABBITS].Add(rabbitsValue);
		}

		private void ObtainMortalityRateData()
		{
			var wolvesValue = GetEntitiesBySpeciesInDeaths(Species.Wolf, entityManager.Deaths).Count;
			var rabbitsValue = GetEntitiesBySpeciesInDeaths(Species.Rabbit, entityManager.Deaths).Count;
			var allEntitiesValue = wolvesValue + rabbitsValue;
			mortalityRateData[Constants.ALL_ENTITIES].Add(allEntitiesValue);
			mortalityRateData[Constants.WOLVES].Add(wolvesValue);
			mortalityRateData[Constants.RABBITS].Add(rabbitsValue);
		}

		private void ObtainSpeedEvolutionData()
		{
			var wolvesValue = GetAverageSpeed(GetEntitiesBySpecies(Species.Wolf, entityManager.Entities));
			var rabbitsValue = GetAverageSpeed(GetEntitiesBySpecies(Species.Rabbit, entityManager.Entities));
			var allEntitiesValue = (wolvesValue + rabbitsValue) / 2;
			speedEvolutionData[Constants.ALL_ENTITIES].Add(allEntitiesValue);
			speedEvolutionData[Constants.WOLVES].Add(wolvesValue);
			speedEvolutionData[Constants.RABBITS].Add(rabbitsValue);
		}

		private void ObtainSensoryDistanceEvolutionData()
		{
			var wolvesValue = GetAverageSensoryDistance(GetEntitiesBySpecies(Species.Wolf, entityManager.Entities));
			var rabbitsValue = GetAverageSensoryDistance(GetEntitiesBySpecies(Species.Rabbit, entityManager.Entities));
			var allEntitiesValue = (wolvesValue + rabbitsValue) / 2;
			sensoryDistanceData[Constants.ALL_ENTITIES].Add(allEntitiesValue);
			sensoryDistanceData[Constants.WOLVES].Add(wolvesValue);
			sensoryDistanceData[Constants.RABBITS].Add(rabbitsValue);
		}

		private void ObtainDesirabilityEvolutionData()
		{
			var wolvesValue = GetAverageDesirability(GetEntitiesBySpecies(Species.Wolf, entityManager.Entities));
			var rabbitsValue = GetAverageDesirability(GetEntitiesBySpecies(Species.Rabbit, entityManager.Entities));
			var allEntitiesValue = (wolvesValue + rabbitsValue) / 2;
			desirabilityData[Constants.ALL_ENTITIES].Add(allEntitiesValue);
			desirabilityData[Constants.WOLVES].Add(wolvesValue);
			desirabilityData[Constants.RABBITS].Add(rabbitsValue);
		}

		private List<Entity> GetEntitiesBySpecies(Species species, Dictionary<int, Entity> entities)
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

		private List<Entity> GetEntitiesBySpeciesInDeaths(Species species, Dictionary<CauseOfDeath, List<Entity>> entities)
		{
			var entitiesBySpecies = new List<Entity>();
			foreach (CauseOfDeath causeOfDeath in Enum.GetValues(typeof(CauseOfDeath)))
			{
				foreach (var entity in entities[causeOfDeath])
				{
					if (entity.species == species)
					{
						entitiesBySpecies.Add(entity);
					}
				}
			}
			return entitiesBySpecies;
		}

		private float GetAverageSpeed(List<Entity> entities)
		{
			var average = 0f;
			var count = 0;
			if (entities.Count == 0)
			{
				return 0;
			}
			foreach (var entity in entities)
			{
				average += entity.GetComponent<Animal>().Speed;
				count++;
			}
			return (float)Math.Round(average / count,2);
		}

		private float GetAverageSensoryDistance(List<Entity> entities)
		{
			var average = 0f;
			var count = 0;
			if (entities.Count == 0)
			{
				return 0;
			}
			foreach (var entity in entities)
			{
				average += entity.GetComponent<Animal>().SensoryDistance;
				count++;

			}
			return (float)Math.Round(average / count, 2);
		}

		private float GetAverageDesirability(List<Entity> entities)
		{
			var average = 0f;
			var count = 0;
			if (entities.Count == 0)
			{
				return 0;
			}
			foreach (var entity in entities)
			{
				if (entity.GetComponent<Animal>().isMale)
				{
					average += entity.GetComponent<Animal>().desirability;
					count++;
				}
			}
			if(count == 0)
			{
				return 0;
			}
			return (float)Math.Round(average / count, 2);
		}
	}
}