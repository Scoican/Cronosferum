/*using DG.Tweening;
using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
	[HideInInspector]
	public Tile currentTile;

	private Animal animal;
	private MapManager map;
	private List<Node> path;

	private void Start()
	{
		map = GameObject.Find("Map").GetComponent<MapManager>();
		animal = GetComponent<Animal>();
		InvokeRepeating("HandleMovement", 0.5f, animal.Speed);
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
			case EntityState.SatisfyReproductionUrge:
				break;
			case EntityState.Fleeing:
				Wander();
				break;
		}
	}

	private void Wander()
	{
		var possibleDestinations = GetPossibleDestinations();
		if (possibleDestinations.Count == 0)
		{
			return;
		}
		var randomIndex = UnityEngine.Random.Range(0, possibleDestinations.Count);
		var nextTile = possibleDestinations[randomIndex];
		MoveToTarget(nextTile);
	}

	private void HeadToFood()
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
		else if (path.Count == 0)
		{
			animal.currentState = EntityState.Wandering;
			path = null;
			animal.foodTarget = null;
		}
	}

	private void HeadForWater()
	{
		if (animal.WaterTarget == null || animal.WaterTarget == Position.invalid)
		{
			animal.WaterTarget = animal.SenseWater(transform.position, 2);
		}

		if (path != null)
		{
			var nextTile = map.GetTile(path[0].position);
			path.RemoveAt(0);
			MoveToTarget(nextTile);
		}
		else if (animal.WaterTarget != Position.invalid)
		{
			path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.WaterTarget);
			if (path.Count > 1)
			{
				path.RemoveAt(path.Count - 1);
			}
		}

		if (path == null)
		{
			Wander();
		}
		else if (path.Count == 0)
		{
			animal.currentState = EntityState.Wandering;
			path = null;
			animal.WaterTarget = Position.invalid;
		}
	}

	private void MoveToTarget(Tile target)
	{
		RotateToFaceNextDestination(target);
		transform.DOJump(new Vector3(target.Position.x, target.Height / 10, target.Position.y), 0.5f, 0, 0.25f);
		currentTile.Occupied = false;
		currentTile = target;
		currentTile.Occupied = true;
		animal.position = currentTile.Position;
	}

	private List<Tile> GetPossibleDestinations()
	{
		var possibleDestinations = map.GetTileNeighbours(currentTile.Position);
		for (int i = 0; i < possibleDestinations.Count; i++)
		{
			if (possibleDestinations[i].Type == Tile.TileType.Water || possibleDestinations[i].Occupied == true)
			{
				possibleDestinations.RemoveAt(i);
				i--;
			}
		}
		return possibleDestinations;
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

	private Tile getRandomTile()
	{
		var xCoord = (int)UnityEngine.Random.Range(0, map.Size.x);
		var yCoord = (int)UnityEngine.Random.Range(0, map.Size.y);
		var randomTile = map.GetTile(new Position(xCoord, yCoord));
		if (randomTile.Type == Tile.TileType.Water)
		{
			return getRandomTile();
		}
		else
		{
			return randomTile;
		}
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(1))
		{
			EntityManager.Instance.Unregister(GetComponent<Animal>());
			currentTile.Occupied = false;
			Destroy(gameObject);
		}
	}
}
*/