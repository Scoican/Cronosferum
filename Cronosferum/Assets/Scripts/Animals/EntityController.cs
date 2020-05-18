using DG.Tweening;
using Predation;
using Predation.Utils;
using System;
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
			case EntityState.Survive:
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
		if (animal.foodTarget == null || animal.foodTarget == Position.invalid)
		{
			animal.foodTarget = animal.SenseFood(transform.position, 2);
		}

		if (path != null && animal.foodTarget != Position.invalid)
		{
			path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.foodTarget);
			var nextTile = map.GetTile(path[0].position);
			MoveToTarget(nextTile);
		}
		else if (animal.foodTarget != Position.invalid)
		{
			path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.foodTarget);
		}

		if (path == null)
		{
			Wander();
		}
		else if (path.Count == 0)
		{
			animal.currentState = EntityState.Wandering;
			path = null;
			animal.foodTarget = Position.invalid;
		}
	}

	private void HeadForWater()
	{
		if (animal.waterTarget == null || animal.waterTarget == Position.invalid)
		{
			animal.waterTarget = animal.SenseWater(transform.position, 2);
		}

		if (path != null)
		{
			var nextTile = map.GetTile(path[0].position);
			path.RemoveAt(0);
			MoveToTarget(nextTile);
		}
		else if (animal.waterTarget != Position.invalid)
		{
			path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.waterTarget);
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
			animal.waterTarget = Position.invalid;
		}
	}

	private void MoveToTarget(Tile target)
	{
		RotateToFaceNextDestination(target);
		transform.DOJump(new Vector3(target.MapPosition.x, target.Height / 10, target.MapPosition.y), 0.5f, 0, 0.25f);
		currentTile.Occupied = false;
		currentTile = target;
		currentTile.Occupied = true;
		animal.position = currentTile.MapPosition;
	}

	private List<Tile> GetPossibleDestinations()
	{
		var possibleDestinations = map.GetTileNeighbours(currentTile.MapPosition);
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
		if (currentTile.MapPosition.x > destination.MapPosition.x)
		{
			transform.rotation = Quaternion.Euler(0f, -90f, 0f);
			return;
		}
		if (currentTile.MapPosition.y > destination.MapPosition.y)
		{
			transform.rotation = Quaternion.Euler(0f, 180f, 0f);
			return;
		}
		if (currentTile.MapPosition.x < destination.MapPosition.x)
		{
			transform.rotation = Quaternion.Euler(0f, 90f, 0f);
			return;
		}
		if (currentTile.MapPosition.y < destination.MapPosition.y)
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
