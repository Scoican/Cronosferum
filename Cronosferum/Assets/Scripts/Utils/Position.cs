using UnityEngine;

namespace Predation.Utils
{
	[System.Serializable]
	public struct Position
	{

		public int x;
		public int y;

		public Position(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public static int SqrDistance(Position a, Position b)
		{
			return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
		}

		public static float Distance(Position a, Position b)
		{
			return (float)System.Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
		}

		public static bool AreNeighbours(Position a, Position b)
		{
			return System.Math.Abs(a.x - b.x) <= 1 && System.Math.Abs(a.y - b.y) <= 1;
		}

		public static Position invalid
		{
			get
			{
				return new Position(-1, -1);
			}
		}

		public static Position up
		{
			get
			{
				return new Position(0, 1);
			}
		}

		public static Position down
		{
			get
			{
				return new Position(0, -1);
			}
		}

		public static Position left
		{
			get
			{
				return new Position(-1, 0);
			}
		}

		public static Position right
		{
			get
			{
				return new Position(1, 0);
			}
		}

		public static Position operator +(Position a, Position b)
		{
			return new Position(a.x + b.x, a.y + b.y);
		}

		public static Position operator -(Position a, Position b)
		{
			return new Position(a.x - b.x, a.y - b.y);
		}

		public static bool operator ==(Position a, Position b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(Position a, Position b)
		{
			return a.x != b.x || a.y != b.y;
		}

		public static implicit operator Vector2(Position v)
		{
			return new Vector2(v.x, v.y);
		}

		public static implicit operator Vector3(Position v)
		{
			return new Vector3(v.x, 0, v.y);
		}

		public override bool Equals(object other)
		{
			return (Position)other == this;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public override string ToString()
		{
			return "(" + x + " ; " + y + ")";
		}
	}
}
