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

	private Vector2 size;

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
		Tiles = builder.GenerateMap(size.x, size.y);
		MapGraph = GetComponent<MapGraph>();
		MapGraph.GenerateGraph(this);
	}

	public Vector3 Size
	{
		get
		{
			return size;
		}
		set
		{
			size = value;
		}
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
		var neighboursCoordinates = new List<Position>() { Position.down,Position.up,Position.left,Position.right};
		foreach (var possibleNeighbour in neighboursCoordinates)
		{
			if (Tiles.ContainsKey(coordinates + possibleNeighbour))
			{
				neighbours.Add(Tiles[coordinates + possibleNeighbour]);
			}
		}
		return neighbours;
	}

	public void RegenerateMap()
	{
		ClearMap();
		Tiles = builder.GenerateMap(size.x, size.y);
	}
}
