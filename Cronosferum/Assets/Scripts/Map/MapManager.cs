using Predation.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	private static MapManager instance;

	public static MapManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<MapManager>();
				if (instance == null)
				{
					var container = new GameObject("MapManager");
					instance = container.AddComponent<MapManager>();
				}
			}
			return instance;
		}
	}
	public Vector2 Size;

	public MapGraph MapGraph;
	public MapBuilder builder;
	[HideInInspector]
	public Dictionary<Position, Tile> Tiles = new Dictionary<Position, Tile>();

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("More than one MapManager in the scene!");
			Destroy(gameObject);
		}
	}

	public void InitializeMap()
	{
		if (GameSettings.MapSize != 0)
		{
			Tiles = builder.GenerateMap(GameSettings.MapSize, GameSettings.MapSize);
		}
		else
		{
			Tiles = builder.GenerateMap(Size.x, Size.y);
		}

		MapGraph.GenerateGraph(this);
	}

	public void ClearMap()
	{
		Tiles.Values.ToList().ForEach(tile => Destroy(tile.gameObject));
		Tiles.Clear();
	}

	public void SetTile(Position coordinates, Tile tile)
	{
		if (Tiles.ContainsKey(coordinates))
			Tiles[coordinates] = tile;
		else
			Tiles.Add(coordinates, tile);
	}

	public Tile GetTile(Position coordinates)
	{
		if (Tiles.ContainsKey(coordinates))
			return Tiles[coordinates];
		return null;
	}

	public void RemoveTile(Position coordinates)
	{
		if (!Tiles.ContainsKey(coordinates))
			return;
		Destroy(Tiles[coordinates].gameObject);
	}

	public List<Tile> GetTileNeighbours(Position coordinates)
	{
		var neighbours = new List<Tile>();
		var neighboursCoordinates = new List<Position>() { Position.down, Position.up, Position.left, Position.right };
		foreach (var possibleNeighbour in neighboursCoordinates)
		{
			if (Tiles.ContainsKey(coordinates + possibleNeighbour))
			{
				neighbours.Add(Tiles[coordinates + possibleNeighbour]);
			}
		}
		return neighbours;
	}

	public Dictionary<Position, Tile> GetUnoccupiedTiles()
	{
		var unocupiedTiles = new Dictionary<Position, Tile>();
		foreach(var tile in Tiles)
		{
			if(!tile.Value.Occupied && tile.Value.Type != Tile.TileType.Water)
			{
				unocupiedTiles.Add(tile.Key,tile.Value);
			}
		}
		return unocupiedTiles;
	}

	public void RegenerateMap()
	{
		ClearMap();
		Tiles = builder.GenerateMap(GameSettings.MapSize, GameSettings.MapSize);
	}
}
