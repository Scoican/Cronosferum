using Predation.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Entity
{
	public Species foodPreference;

	public Color maleColour;
	public Color femaleColour;

	public float Speed = 2f;
	float timeToDeathByHunger = 200;
	float timeToDeathByThirst = 200;

	[Header("State")]
	public float hunger;
	public float thirst;
	public EntityState currentState;

	public Position foodTarget;
	public Position waterTarget;

	private void Start()
	{
		foodTarget = Position.invalid;
		waterTarget = Position.invalid;
	}
	private void Update()
	{
		// Increase hunger and thirst over time
		hunger += Time.deltaTime * 1 / timeToDeathByHunger;
		thirst += Time.deltaTime * 1 / timeToDeathByThirst;

		if (currentState == EntityState.Wandering && (hunger > 0.25f || thirst >= 0.25f))
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
		if (hunger > thirst)
		{
			currentState = EntityState.SatisfyHunger;
		}
		else
		{
			currentState = EntityState.SatisfyThirst;
		}
		Debug.Log("My current state is:" + currentState);
	}

	public Position SenseFood(Vector3 center, float radius)
	{
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);
		var closeEnties = new List<Position>();
		foreach (var objectCollided in hitColliders)
		{
			if (objectCollided.gameObject.layer == LayerMask.NameToLayer("Interactible"))
			{
				if (objectCollided.GetComponent<Entity>().species == foodPreference)
				{
					closeEnties.Add(objectCollided.GetComponent<Entity>().position);
				}
			}
		}
		if (closeEnties.Count != 0)
		{
			return GetClosestPosition(closeEnties);
		}
		else
		{
			return Position.invalid;
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
				closestWaterSource.Add(objectCollided.GetComponent<Tile>().MapPosition);
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

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 2);
	}
}
