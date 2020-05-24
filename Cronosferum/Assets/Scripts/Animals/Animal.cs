using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Entity
{
	[Header("Traits")]
	public Species foodPreference;
	public Sex animalSex;
	public float Speed;
	public float desirability;

	[Header("State")]
	public float hunger;
	public float thirst;
	public float reproductionUrge;
	public EntityState currentState;

	[Header("Targets")]
	public Entity foodTarget;
	public Position WaterTarget;
	public Animal mateTarget;

	[Header("Color materials")]
	public Material maleMaterial;
	public Material femaleMaterial;

	private float maximumReproductionUrge = 200;

	private void Start()
	{
		InitializeAnimal();
	}

	private void InitializeAnimal()
	{
		foodTarget = null;
		WaterTarget = Position.invalid;
		if (Random.Range(0f, 1f) <= 0.5)
		{
			animalSex = Sex.Male;
			GetComponentInChildren<SkinnedMeshRenderer>().material = maleMaterial;
			desirability = Random.Range(0f, 1f);
			GetComponentInChildren<SkinnedMeshRenderer>().material.color += new Color(maleMaterial.color.r * desirability, 0, 0);
		}
		else
		{
			animalSex = Sex.Female;
			GetComponentInChildren<SkinnedMeshRenderer>().material = femaleMaterial;
		}
	}

	private void Update()
	{
		reproductionUrge += Time.deltaTime * 1 / maximumReproductionUrge;
		if (currentState == EntityState.Wandering && (hunger > 0.25f || thirst >= 0.25f || reproductionUrge >= 0.5f))
		{
			HandleState();
		}

		if (hunger >= 1)
		{
			Die(CauseOfDeath.Hunger);
		}
		else if (thirst >= 1)
		{
			Die(CauseOfDeath.Thirst);
		}
	}

	private void HandleState()
	{
		if (reproductionUrge >= 0.5f && reproductionUrge > hunger && reproductionUrge > thirst && animalSex == Sex.Male)
		{
			currentState = EntityState.LookingForMate;
		}
		else
		{
			if (hunger > thirst)
			{
				currentState = EntityState.SatisfyHunger;
			}
			else
			{
				currentState = EntityState.SatisfyThirst;
			}
		}
		Debug.Log("My current state is:" + currentState);
	}

	public Entity SenseFood(Vector3 center, float radius)
	{
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);
		var closeEnties = new List<Entity>();
		foreach (var objectCollided in hitColliders)
		{
			if (objectCollided.gameObject.layer == LayerMask.NameToLayer("Interactible"))
			{
				if (objectCollided.GetComponent<Entity>().species == foodPreference)
				{
					closeEnties.Add(objectCollided.GetComponent<Entity>());
				}
			}
		}
		if (closeEnties.Count != 0)
		{
			return GetClosestEntity(closeEnties);
		}
		else
		{
			return null;
		}
	}

	public Position SenseWater(Vector3 center, float radius)
	{
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);
		var closestWaterSource = new List<Position>();
		foreach (var objectCollided in hitColliders)
		{
			if (objectCollided.gameObject.layer == LayerMask.NameToLayer("Water"))
			{
				closestWaterSource.Add(objectCollided.GetComponent<Tile>().Position);
			}
		}
		if (closestWaterSource.Count != 0)
		{
			return GetClosestPosition(closestWaterSource);
		}
		else
		{
			return Position.invalid;
		}
	}

	public Animal SenseMate(Vector3 center, float radius)
	{
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);
		var closeEnties = new List<Entity>();
		foreach (var objectCollided in hitColliders)
		{
			if (objectCollided.gameObject.layer == LayerMask.NameToLayer("Interactible"))
			{
				if (objectCollided.GetComponent<Animal>().species == species)
				{
					closeEnties.Add(objectCollided.GetComponent<Entity>());
				}
			}
		}
		if (closeEnties.Count != 0)
		{
			var potentialMate = GetClosestEntity(closeEnties).GetComponent<Animal>();
			if (potentialMate.RequestMate(this))
			{
				potentialMate.currentState = EntityState.WaitingForMalePartener;
				return potentialMate;
			}
		}
		return null;
	}

	public void PotentialMateFound(Animal femaleMate)
	{
		bool accepted = femaleMate.RequestMate(this);

		if (accepted)
		{
			mateTarget = femaleMate;
		}
		else
		{
			//TODO
		}

	}

	public bool RequestMate(Animal maleMate)
	{
		float chance = Random.Range(0f, maleMate.desirability);
		if (Random.Range(0f,1f) > chance)
		{
			return false;
		}
		mateTarget = maleMate;
		return true;
	}

	private Position GetClosestPosition(List<Position> entities)
	{
		var minValue = 999999;
		var closestEntityIndex = -1;
		for (int i = 0; i < entities.Count; i++)
		{
			if (Position.SqrDistance(position, entities[i]) < minValue)
			{
				Position.SqrDistance(position, entities[i]);
				closestEntityIndex = i;
			}
		}
		return entities[closestEntityIndex];
	}

	private Entity GetClosestEntity(List<Entity> entities)
	{
		var minValue = 999999;
		var closestEntityIndex = -1;
		for (int i = 0; i < entities.Count; i++)
		{
			if (Position.SqrDistance(position, entities[i].position) < minValue)
			{
				Position.SqrDistance(position, entities[i].position);
				closestEntityIndex = i;
			}
		}
		return entities[closestEntityIndex];
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 2);
	}
}
