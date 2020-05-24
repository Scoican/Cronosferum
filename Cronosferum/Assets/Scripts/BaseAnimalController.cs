using DG.Tweening;
using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BaseAnimalController : MonoBehaviour
{
	[HideInInspector]
	public Tile currentTile;

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
		yield return new WaitForSeconds(animal.Speed);
		while (true)
		{
			HandleMovement();
			yield return new WaitForSeconds(animal.Speed);
		}
	}

	protected virtual void HandleMovement() { }


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
		if (animal.mateTarget == null)
		{
			animal.mateTarget = animal.SenseMate(transform.position, 2);
		}

		if (path != null && animal.mateTarget != null)
		{
			animal.currentState = EntityState.HeadingToFemalePartener;
			animal.mateTarget.currentState = EntityState.WaitingForMalePartener;
			path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.mateTarget.position);
			if (path.Count > 0)
			{
				var nextTile = map.GetTile(path[0].position);
				MoveToTarget(nextTile);
			}
		}
		else if (animal.mateTarget != null)
		{
			animal.currentState = EntityState.HeadingToFemalePartener;
			animal.mateTarget.currentState = EntityState.WaitingForMalePartener;
			path = MapManager.Instance.MapGraph.AStarSearch(animal.position, animal.mateTarget.position);
		}

		if (path == null)
		{
			Wander();
		}
		else if (path.Count == 1)
		{
			animal.currentState = EntityState.Breeding;
			Reproduce(animal.mateTarget);
			animal.currentState = EntityState.Wandering;
			path = null;
			animal.mateTarget = null;
			animal.mateTarget.mateTarget = null;
		}
	}

	public void WaitForPartener()
	{
		var relativePos = animal.mateTarget.transform.position;
		relativePos.y = transform.position.y;
		transform.LookAt(relativePos);
	}

	private void Reproduce(Animal mateTarget)
	{
		Debug.Log("I REPRODUCED");
	}

	public void HeadForWater()
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
			path.RemoveAt(path.Count - 1);
		}

		if (path == null)
		{
			Wander();
		}
		else if (path.Count == 0)
		{
			animal.currentState = EntityState.Eating;
			DrinkWater(map.GetTile(animal.WaterTarget));
			animal.currentState = EntityState.Wandering;
			path = null;
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
			if (possibleDestinations[i].Type == Tile.TileType.Water || possibleDestinations[i].Occupied == true)
			{
				possibleDestinations.RemoveAt(i);
				i--;
			}
		}
		return possibleDestinations;
	}

	protected void MoveToTarget(Tile target)
	{
		RotateToFaceNextDestination(target);
		transform.DOJump(new Vector3(target.Position.x, target.Height / 10, target.Position.y), 0.5f, 0, 0.25f);
		currentTile.Occupied = false;
		currentTile = target;
		currentTile.Occupied = true;
		animal.position = currentTile.Position;

		// Increase hunger and thirst over time
		animal.hunger += 5f / timeToDeathByHunger;
		animal.thirst += 7.5f / timeToDeathByThirst;
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
		if (Input.GetMouseButtonDown(1))
		{
			EntityManager.Instance.Unregister(GetComponent<Animal>());
			currentTile.Occupied = false;
			Destroy(gameObject);
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