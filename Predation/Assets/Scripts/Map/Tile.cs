using Predation.Utils;
using UnityEngine;

namespace Predation.Map
{
	public class Tile : MonoBehaviour
	{
		private bool occupied;
		public bool Occupied
		{
			get
			{
				return EntitiesCount > 0;
			}
			set
			{
				occupied = value;
			}
		}

		public bool HasFood { get; set; }
		public bool CanGrowFood { get; set; }
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

		public int EntitiesCount { get; set; } = 0;

		public bool HasObstacle { get; set; } = false;

		public enum TileType
		{
			Water, Field, Mountain
		}
	}
}
