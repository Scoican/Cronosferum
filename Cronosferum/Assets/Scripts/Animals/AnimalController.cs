using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
	public GameObject animalPrefab;
	public MapGeneration map;

	private GameObject chicken;
	private Tile currentTile;

	void Start()
	{
		chicken = Instantiate(animalPrefab, new Vector3(map.Tiles[0][0].MapPosition.x, map.Tiles[0][0].Height / 10, map.Tiles[0][0].MapPosition.y), Quaternion.identity);
		currentTile = map.Tiles[0][0];
		InvokeRepeating("Move", 3f, 1f);
	}

	private void Move()
	{
		var possibleDestinations = GetPossibleDestinations();
		var nextTile = possibleDestinations[UnityEngine.Random.Range(0, possibleDestinations.Count)];
		chicken.transform.position = new Vector3(nextTile.MapPosition.x, nextTile.Height / 10, nextTile.MapPosition.y);
		currentTile = nextTile;
	}

	private List<Tile> GetPossibleDestinations()
	{
		var possibleDestinations = new List<Tile>();
		var x = (int)currentTile.MapPosition.x;
		var y = (int)currentTile.MapPosition.y;
		if ((y - 1) > 0 && map.Tiles[x][y - 1].Type != Tile.TileType.Water)
		{
			possibleDestinations.Add(map.Tiles[x][y - 1]);
		}
		if ((y + 1) < map.height && map.Tiles[x][y + 1].Type != Tile.TileType.Water)
		{
			possibleDestinations.Add(map.Tiles[x][y + 1]);
		}
		if ((x - 1) > 0 && map.Tiles[x - 1][y].Type != Tile.TileType.Water)
		{
			possibleDestinations.Add(map.Tiles[x - 1][y]);
		}
		if ((x + 1) < map.width && map.Tiles[x + 1][y].Type != Tile.TileType.Water)
		{
			possibleDestinations.Add(map.Tiles[x + 1][y]);
		}
		return possibleDestinations;
	}
}
