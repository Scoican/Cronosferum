using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public bool Walkable { get; set; }
	public bool Occupied { get; set; }
	public Vector2 MapPosition { get; set; }

	public TileType Type { get; set; }

	public float Height
	{
		get
		{
			return transform.localScale.y;
		}
		set
		{
			transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
		}
	}

	public enum TileType
	{
		Water,Field,Mountain
	}
}
