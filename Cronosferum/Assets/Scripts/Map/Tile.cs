using Predation.Utils;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public bool Occupied { get; set; }
	public bool HasFood { get; set; }
	public Position Position { get; set; }

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
