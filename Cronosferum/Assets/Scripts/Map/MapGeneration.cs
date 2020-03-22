using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
	public GameObject tilePrefab;

	//Size of the map in terms of the number of hex tiles
	public int width = 75;
	public int height = 75;
	public float perlinScale = 20f;

	public Tile[][] Tiles;
	private float seed;

	void Start()
	{
		Tiles = new Tile[height][];
		GenerateMap();
	}

	/// <summary>
	/// Method that generates all the tiles for the map
	/// </summary>
	private void GenerateMap()
	{
		seed = UnityEngine.Random.Range(0f, 99999f);
		for (int x = 0; x < width; x++)
		{
			Tiles[x] = new Tile[width];
			for (int z = 0; z < height; z++)
			{
				var newTile = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
				var generatedTile = BuildTile(newTile, x, z);
				generatedTile.transform.SetParent(gameObject.transform);
				Tiles[x][z]=generatedTile;
			}
		}
	}

	private Color32 CalculateTileColor(float tilePerlinValue)
	{
		if (tilePerlinValue < 0.35f)
		{
			tilePerlinValue += 0.1f;
			return new Color32(GetPerlinColor(102, tilePerlinValue), GetPerlinColor(153, tilePerlinValue), GetPerlinColor(255, tilePerlinValue), 20);
		}
		else if (tilePerlinValue >= 0.35f && tilePerlinValue < 0.60f)
		{
			return new Color32(GetPerlinColor(0, tilePerlinValue), GetPerlinColor(204, tilePerlinValue), GetPerlinColor(0, tilePerlinValue), 255);
		}
		else if (tilePerlinValue >= 0.60f && tilePerlinValue <= 1)
		{
			return new Color32(GetPerlinColor(204, tilePerlinValue), GetPerlinColor(102, tilePerlinValue), GetPerlinColor(0, tilePerlinValue), 255);
		}
		return new Color32(102, 153, 255, 20);
	}

	/// <summary>
	/// Methot that calculates a byte for one of the 4 components of the color32
	/// </summary>
	/// <param name="colorValue">Color value desired</param>
	/// <param name="perlinSample">Perlin value modifier</param>
	/// <returns></returns>
	private byte GetPerlinColor(float colorValue, float perlinSample)
	{
		return (byte)((perlinSample * colorValue) + (perlinSample * colorValue) * 0.2f);
	}

	/// <summary>
	/// Method that calculates the Perlin Value of a tile and based on that value returns a float number representing the height
	/// </summary>
	/// <param name="xPos">Position on the map</param>
	/// <param name="zPos">Position on the map</param>
	/// <returns>Heigth of the tile</returns>
	private float CalculateTilePerlinValue(int xPos, int zPos)
	{
		var xPerlinCoord = (float)xPos / 256 * perlinScale + seed;
		var zPerlinCoord = (float)zPos / 256 * perlinScale + seed;

		float sample = Mathf.PerlinNoise(xPerlinCoord, zPerlinCoord);
		return sample;
	}

	/// <summary>
	/// Method that calculates the height of a given tile and calculates a color based on it's Perlin value
	/// </summary>
	/// <param name="tile">Given tile to be modified</param>
	/// <param name="xPos">Position on the map</param>
	/// <param name="zPos">Position on the map</param>
	private Tile BuildTile(GameObject newTile, int xPos, int zPos)
	{
		var tile = newTile.GetComponent<Tile>();
		tile.MapPosition = new Vector2(xPos, zPos);
		var tilePerlinValue = CalculateTilePerlinValue(xPos, zPos);
		var tileColor = CalculateTileColor(tilePerlinValue);
		tile.Type = CalculateTileType(tilePerlinValue);
		tile.Height = tilePerlinValue * 75;
		tile.GetComponentInChildren<Renderer>().material.SetColor("_Color", tileColor);
		return tile;
	}

	private Tile.TileType CalculateTileType(float tilePerlinValue)
	{
		if (tilePerlinValue < 0.35f)
		{
			return Tile.TileType.Water;
		}
		else if (tilePerlinValue >= 0.35f && tilePerlinValue < 0.60f)
		{
			return Tile.TileType.Field;
		}
		else if (tilePerlinValue >= 0.60f && tilePerlinValue <= 1)
		{
			return Tile.TileType.Mountain;
		}
		return Tile.TileType.Water;
	}
}
