using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
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

	public EntityBlueprint entityBlueprint;

	private static int currentID = 0;
	private Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("More than one EntityManager in the scene!");
			Destroy(gameObject);
		}

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
		entities.Add(entity.Id,entity);
		currentID++;
	}

	public void Unregister(Entity entity)
	{
		if (entity == null)
			return;
		if (entities.ContainsKey(entity.Id))
			entities.Remove(entity.Id);
	}

	public List<Entity> GetEntitiesBySpecies(Species species)
	{
		var entitiesBySpecies = new List<Entity>();
		foreach(var entity in entities.Values)
		{
			if(entity.species == species)
			{
				entitiesBySpecies.Add(entity);
			}
		}
		return entitiesBySpecies;
	}
}
