using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Predation.Managers;
using Predation.Utils;
using Predation.Map;

namespace Predation.Entities
{
	public class BaseAnimalController : MonoBehaviour
	{
		[HideInInspector]
		public Tile currentTile;

		private Tile lastVisitedTile;
		protected Animal animal;
		protected List<Node> path;
		protected MapManager map;

		private float timeToDeathByHunger = 200;
		private float timeToDeathByThirst = 200;

		private void Start()
		{
			map = MapManager.Instance;
			animal = GetComponent<Animal>();
			StartCoroutine(Move());
		}

		private IEnumerator Move()
		{
			yield return new WaitForSeconds(animal.Speed / GameManager.GameTimeSpeed + Random.Range(0f, 1f));
			while (true)
			{
				if (GameManager.gameState == GameStates.Running || GameManager.gameState == GameStates.Continued)
				{
					if (animal.currentState == EntityState.Eating || animal.currentState == EntityState.Drinking || animal.currentState == EntityState.Breeding)
					{
						animal.currentState = EntityState.Wandering;
						yield return new WaitForSeconds(animal.Speed / GameManager.GameTimeSpeed * 1.5f);
					}
					HandleMovement();
					yield return new WaitForSeconds(animal.Speed / GameManager.GameTimeSpeed);
				}
				else
				{
					yield return new WaitForSeconds(0.5f);
				}

			}
		}

		private void HandleMovement()
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
				case EntityState.WaitingForMalePartener:
					WaitForPartener();
					break;
				case EntityState.Idle:
					break;
				default:
					Wander();
					break;
			}
		}

		protected virtual void HeadToFood() { }

		protected virtual void ConsumeFood(Entity foodTarget) { }

		protected void Wander()
		{
			var possibleDestinations = GetPossibleDestinations();
			if (possibleDestinations.Count == 0)
			{
				return;
			}
			var randomIndex = Random.Range(0, possibleDestinations.Count);
			var nextTile = possibleDestinations[randomIndex];
			MoveToTarget(nextTile);
		}

		public void HeadToMate()
		{
			var partener = animal.mateTarget;
			if (partener == null)
			{
				partener = animal.SenseMate(transform.position);
			}

			if (path != null && partener != null)
			{
				animal.currentState = EntityState.HeadingToFemalePartener;
				partener.currentState = EntityState.WaitingForMalePartener;
				path = MapManager.Instance.MapGraph.AStarSearch(animal.position, partener.position);
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
			else if (partener != null)
			{
				path = MapManager.Instance.MapGraph.AStarSearch(animal.position, partener.position);
			}

			if (path == null || animal.mateTarget == null)
			{
				Wander();
			}
			else if (path.Count <= 1 && partener != null)
			{
				animal.currentState = EntityState.Breeding;
				partener.currentState = EntityState.Breeding;
				if (partener.mateTarget == null)
				{
					partener.mateTarget = animal;
				}
				partener.GetComponent<BaseAnimalController>().Reproduce();
				path = null;
				animal.mateTarget = null;
				animal.reproductionUrge = 0;
			}
		}

		public void WaitForPartener()
		{
			if (animal.mateTarget == null)
			{
				animal.currentState = EntityState.Wandering;
				return;
			}
			var relativePos = animal.mateTarget.transform.position;
			relativePos.y = transform.position.y;
			transform.LookAt(relativePos);
		}

		public void Reproduce()
		{
			var tile = map.GetTile(animal.position);
			var offspring = tile.GetComponent<TileController>().SpawnEntity(EntityFactory.Instance.getEntity(animal.species), true);
			offspring.GetComponent<Animal>().SetGenes(Genes.InheritedGenes(animal.Genes, animal.mateTarget.Genes));
			animal.mateTarget = null;
			animal.reproductionUrge = 0;
		}

		public void HeadForWater()
		{
			if (animal.WaterTarget == Position.invalid)
			{
				var possibleNewWaterTarget = animal.SenseWater(transform.position);
				if (possibleNewWaterTarget != Position.invalid)
				{
					if (animal.LastWaterTarget != Position.invalid && Position.Distance(animal.position, possibleNewWaterTarget) > Position.Distance(animal.position, animal.LastWaterTarget))
					{
						animal.WaterTarget = animal.LastWaterTarget;
					}
					else
					{
						animal.WaterTarget = possibleNewWaterTarget;
					}
				}
				else if (animal.LastWaterTarget != Position.invalid)
				{
					animal.WaterTarget = animal.LastWaterTarget;
				}
				else
				{
					Wander();
				}
			}
			if (path != null && animal.WaterTarget != Position.invalid)
			{
				if (path.Count > 0)
				{
					var nextTile = map.GetTile(path[0].position);
					path.RemoveAt(0);
					MoveToTarget(nextTile);
				}
			}
			else if (animal.WaterTarget != Position.invalid)
			{
				path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.WaterTarget);
			}
			if (path == null || animal.WaterTarget == Position.invalid)
			{
				Wander();
			}
			else if (path.Count == 0 && animal.WaterTarget != Position.invalid)
			{
				animal.currentState = EntityState.Drinking;
				DrinkWater(map.GetTile(animal.WaterTarget));
				path = null;
				animal.LastWaterTarget = animal.WaterTarget;
				animal.WaterTarget = Position.invalid;
			}
		}

		private void DrinkWater(Tile waterTile)
		{
			var relativePos = waterTile.transform.position;
			relativePos.y = transform.position.y;
			transform.LookAt(relativePos);
			animal.thirst = 0;
		}

		private List<Tile> GetPossibleDestinations()
		{
			var possibleDestinations = map.GetTileNeighbours(currentTile.Position);
			for (int i = 0; i < possibleDestinations.Count; i++)
			{
				if (possibleDestinations[i].Type == Tile.TileType.Water || possibleDestinations[i].HasObstacle)
				{
					possibleDestinations.RemoveAt(i);
					i--;
				}
			}
			if (possibleDestinations.Count > 1)
			{
				possibleDestinations.Remove(lastVisitedTile);
			}
			return possibleDestinations;
		}

		protected void MoveToTarget(Tile target)
		{
			RotateToFaceNextDestination(target);
			transform.DOJump(new Vector3(target.Position.x, target.Height / 10, target.Position.y), 0.5f, 0, 0.25f);
			currentTile.EntitiesCount--;
			lastVisitedTile = currentTile;
			currentTile = target;
			currentTile.EntitiesCount++;
			animal.position = currentTile.Position;

			// Increase hunger and thirst over time
			animal.hunger += 5f / timeToDeathByHunger;
			animal.thirst += 2.5f / timeToDeathByThirst;
		}

		private void RotateToFaceNextDestination(Tile destination)
		{
			if (currentTile.Position.x > destination.Position.x)
			{
				transform.rotation = Quaternion.Euler(0f, -90f, 0f);
				return;
			}
			if (currentTile.Position.y > destination.Position.y)
			{
				transform.rotation = Quaternion.Euler(0f, 180f, 0f);
				return;
			}
			if (currentTile.Position.x < destination.Position.x)
			{
				transform.rotation = Quaternion.Euler(0f, 90f, 0f);
				return;
			}
			if (currentTile.Position.y < destination.Position.y)
			{
				transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				return;
			}
		}

		void OnMouseOver()
		{
			if (Input.GetMouseButtonDown(0))
			{
				UIManager.Instance.DisplayAnimalInformation(animal);
				Debug.Log($"I am a {animal.species}, my current state is {animal.currentState}, my targets are: \n" +
					$"Water Target:{animal.WaterTarget}\nFood Target:{animal.foodTarget}\nMate Target:{animal.mateTarget}");
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.X))
			{
				HandleMovement();
			}
		}
	}
}