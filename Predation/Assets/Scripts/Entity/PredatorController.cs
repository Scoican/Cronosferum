using Predation.Managers;
using Predation.Utils;
using UnityEngine;

namespace Predation.Entities
{
	public class PredatorController : BaseAnimalController
	{
		protected override void HeadToFood()
		{
			if (animal.foodTarget == null)
			{
				animal.foodTarget = animal.SenseFood(transform.position);
			}

			if (path != null && animal.foodTarget != null)
			{
				path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.foodTarget.position);
				if (path != null && path.Count > 0)
				{
					var nextTile = map.GetTile(path[0].position);
					MoveToTarget(nextTile);
				}
				else
				{
					Wander();
				}
			}
			else if (animal.foodTarget != null)
			{
				animal.foodTarget.GetComponent<Animal>().currentState = EntityState.Fleeing;
				path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.foodTarget.position);
			}

			if (path == null || animal.foodTarget == null)
			{
				Wander();
			}
			else if (path.Count == 1)
			{
				animal.currentState = EntityState.Eating;
				ConsumeFood(animal.foodTarget);
				path = null;
				animal.foodTarget = null;
			}
		}

		protected override void ConsumeFood(Entity foodTarget)
		{
			var prey = foodTarget.GetComponent<Animal>();
			if (prey == null)
			{
				Debug.LogError("The entity you are trying to consume does not have a Animal.cs script");
				return;
			}
			transform.LookAt(foodTarget.transform);
			animal.hunger = 0;
			animal.thirst -= animal.thirst / 2;
			prey.Die(CauseOfDeath.Eaten);
		}
	}
}