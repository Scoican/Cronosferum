using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Predation.Map
{
	public class MapBuilder : MonoBehaviour
	{
		public GameObject tilePrefab;

		//Size of the map in terms of the number of hex tiles
		[Range(0, 100)]
		public float PerlinScale = 100f;

		[Range(0, 100)]
		public float TileHeightMultiplier = 50f;

		private float seedX;
		private float seedZ;

		/// <summary>
		/// Method that generates all the tiles for the map
		/// </summary>
		public Dictionary<Position, Tile> GenerateMap(float x, float y)
		{
			var Tiles = new Dictionary<Position, Tile>();
			seedX = Random.Range(0f, 99999f);
			seedZ = Random.Range(0f, 99999f);
			for (int i = 0; i < x; i++)
			{
				for (int j = 0; j < y; j++)
				{
					var newTile = Instantiate(tilePrefab, new Vector3(i, 0, j), Quaternion.identity);
					var generatedTile = BuildTile(newTile, i, j);
					generatedTile.transform.SetParent(gameObject.transform);
					Tiles.Add(new Position(i, j), generatedTile);
				}
			}
			return Tiles;
		}

		private Color32 CalculateTileColor(float tilePerlinValue)
		{
			return GetGradient(tilePerlinValue);
		}

		private Color32 GetGradient(float perlinValue)
		{
			var gradient = new Gradient();

			// Populate the color keys at the relative time 0 and 1 (0 and 100%)
			var colorKey = new GradientColorKey[8];
			colorKey[0].color = Constants.WaterColorDark;
			colorKey[1].color = Constants.WaterColorMediumDark;
			colorKey[2].color = Constants.WaterColorLight;

			colorKey[3].color = Constants.HillColorLight;
			colorKey[4].color = Constants.HillColorMediumDark;
			colorKey[5].color = Constants.HillColorDark;

			colorKey[6].color = Constants.MountainColorMediumDark;
			colorKey[7].color = Constants.MountainColorDark;

			colorKey[0].time = 0f;
			colorKey[1].time = 1 / 8f;
			colorKey[2].time = 2 / 8f;

			colorKey[3].time = 3 / 8f;
			colorKey[4].time = 4 / 8f;
			colorKey[5].time = 5 / 8f;

			colorKey[6].time = 6 / 8f;
			colorKey[7].time = 7 / 8f;



			// Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
			var alphaKey = new GradientAlphaKey[8];
			alphaKey[0].alpha = 0.2f;
			alphaKey[1].alpha = 0.2f;
			alphaKey[2].alpha = 1.0f;
			alphaKey[3].alpha = 1.0f;
			alphaKey[4].alpha = 1.0f;
			alphaKey[5].alpha = 1.0f;
			alphaKey[6].alpha = 1.0f;
			alphaKey[7].alpha = 1.0f;

			alphaKey[0].time = 0f;
			alphaKey[1].time = 1 / 8f;
			alphaKey[2].time = 2 / 8f;
			alphaKey[3].time = 3 / 8f;
			alphaKey[4].time = 4 / 8f;
			alphaKey[5].time = 5 / 8f;
			alphaKey[6].time = 6 / 8f;
			alphaKey[7].time = 7 / 8f;

			gradient.SetKeys(colorKey, alphaKey);

			// What's the color at the relative time 0.25 (25 %) ?
			return gradient.Evaluate(perlinValue);
		}

		/// <summary>
		/// Method that calculates the Perlin Value of a tile and based on that value returns a float number representing the height
		/// </summary>
		/// <param name="xPos">Position on the map</param>
		/// <param name="zPos">Position on the map</param>
		/// <returns>Heigth of the tile</returns>
		private float CalculateTilePerlinValue(int xPos, int zPos)
		{
			var xPerlinCoord = (float)xPos / 256 * PerlinScale + seedX;
			var zPerlinCoord = (float)zPos / 256 * PerlinScale + seedZ;

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
			tile.Position = new Position(xPos, zPos);
			var tilePerlinValue = CalculateTilePerlinValue(xPos, zPos);
			var tileColor = CalculateTileColor(tilePerlinValue);
			tile.Type = CalculateTileType(tilePerlinValue);
			if (tile.Type == Tile.TileType.Water)
			{
				tile.gameObject.layer = LayerMask.NameToLayer("Water");
			}
			tile.Height = tilePerlinValue * TileHeightMultiplier;
			if (tile.Type != Tile.TileType.Water && !tile.HasObstacle && Random.Range(0f, 1f) <= GameSettings.OtherElementsPercentage)
			{
				GenerateObstacles(tile);
			}
			if (tile.Type == Tile.TileType.Field && !tile.HasObstacle && Random.Range(0f, 1f) <= GameSettings.FoodPercentage)
			{
				GeneratePlant(tile);
			}
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

		private void GeneratePlant(Tile tile)
		{
			tile.GetComponent<TileController>().SpawnPlant(false);
			tile.CanGrowFood = true;
		}

		private void GenerateObstacles(Tile tile)
		{
			GameObject prefab;

			if (tile.Type == Tile.TileType.Field)
			{
				prefab = Resources.Load(Paths.GREEN_TREES_PREFAB + Random.Range(1, 5).ToString()) as GameObject;
				var obstacle = Instantiate(prefab, new Vector3(tile.Position.x, tile.Height / 10, tile.Position.y), Quaternion.identity);
				obstacle.transform.SetParent(tile.transform);
			}
			else if (tile.Type == Tile.TileType.Mountain)
			{
				prefab = Resources.Load(Paths.ORANGE_TREES_PREFAB + Random.Range(1, 5).ToString()) as GameObject;
				var obstacle = Instantiate(prefab, new Vector3(tile.Position.x, tile.Height / 10, tile.Position.y), Quaternion.identity);
				obstacle.transform.SetParent(tile.transform);
			}
			tile.HasObstacle = true;
		}
	}
}
