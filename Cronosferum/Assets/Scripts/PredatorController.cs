using Predation.Utils;
using UnityEngine;

public class PredatorController : BaseAnimalController,IAnimalController
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
			path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.foodTarget.position);
			if (path.Count > 0)
			{
				var nextTile = map.GetTile(path[0].position);
				MoveToTarget(nextTile);
			}
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
			ConsumePrey(animal.foodTarget);
			animal.currentState = EntityState.Wandering;
			path = null;
			animal.foodTarget = null;
		}
	}

	private void ConsumePrey(Entity foodTarget)
	{
		var prey = foodTarget.GetComponent<Animal>();
		if (prey == null)
		{
			Debug.LogError("The plant you are trying to consume does not have a Plant.cs script");
			return;
		}
		transform.LookAt(foodTarget.transform);
		animal.hunger = 0;
		foodTarget.Die(CauseOfDeath.Eaten);
	}


}
