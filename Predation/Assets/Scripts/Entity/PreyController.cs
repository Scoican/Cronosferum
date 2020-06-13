using Predation.Managers;
using UnityEngine;

namespace Predation.Entities
{
	public class PreyController : BaseAnimalController
	{
		protected override void HeadToFood()
		{
			if (animal.foodTarget == null)
			{
				animal.foodTarget = animal.SenseFood(transform.position);
			}

			if (path != null && animal.foodTarget != null)
			{
				if (path.Count > 0)
				{
					var nextTile = map.GetTile(path[0].position);
					path.RemoveAt(0);
					MoveToTarget(nextTile);
				}
			}
			else if (animal.foodTarget != null)
			{
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
			var plant = foodTarget.GetComponent<Plant>();
			if (plant == null)
			{
				Debug.LogError("The plant you are trying to consume does not have a Plant.cs script");
				return;
			}
			transform.LookAt(foodTarget.transform);
			animal.hunger -= plant.Consume(animal.hunger);
			animal.thirst -= animal.thirst / 4;
			if (plant.AmountRemaining <= 0)
			{
				map.GetTile(foodTarget.position).HasFood = false;
			}
		}
	}
}