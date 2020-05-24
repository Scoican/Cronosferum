using System;
using Predation.Utils;
using UnityEngine;

public class PreyController : BaseAnimalController, IAnimalController
{
	protected override void HandleMovement()
	{
		switch (animal.currentState)
		{
			case EntityState.Wandering:
				Wander();
				break;
			case EntityState.SatisfyHunger:
				HeadToFood();
				break;
			case EntityState.SatisfyThirst:
				HeadForWater();
				break;
			case EntityState.LookingForMate:
			case EntityState.HeadingToFemalePartener:
				HeadToMate();
				break;
			case EntityState.Fleeing:
				Wander();
				break;
			case EntityState.WaitingForMalePartener:
				WaitForPartener();
				break;
			default:
				Wander();
				break;
		}
	}

	public void HeadToFood()
	{
		if (animal.foodTarget == null)
		{
			animal.foodTarget = animal.SenseFood(transform.position, 2);
		}

		if (path != null && animal.foodTarget != null)
		{
			var nextTile = map.GetTile(path[0].position);
			path.RemoveAt(0);
			MoveToTarget(nextTile);
		}
		else if (animal.foodTarget != null)
		{
			path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.foodTarget.position);
		}

		if (path == null)
		{
			Wander();
		}
		else if (path.Count == 1)
		{
			animal.currentState = EntityState.Eating;
			ConsumePlant(animal.foodTarget);
			animal.currentState = EntityState.Wandering;
			path = null;
			animal.foodTarget = null;
		}
	}

	private void ConsumePlant(Entity foodTarget)
	{
		var plant = foodTarget.GetComponent<Plant>();
		if (plant == null)
		{
			Debug.LogError("The plant you are trying to consume does not have a Plant.cs script");
			return;
		}
		transform.LookAt(foodTarget.transform);
		animal.hunger -= plant.Consume(animal.hunger);
	}
}