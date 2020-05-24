using UnityEngine;

public static class GameSettings
{
	private static int mapSize = 30;
	private static float foodPercentage = 0;
	private static float otherElementsPercentage = 0;
	public static bool HasBeenModified = false;

	public static int MapSize
	{
		get
		{
			return mapSize;
		}
		set
		{
			switch (value)
			{
				case 1:
					mapSize = 30;
					break;
				case 2:
					mapSize = 40;
					break;
				case 3:
					mapSize = 50;
					break;
				default:
					mapSize = 50;
					break;
			}
		}
	}

	public static int PreyPopulation { get; set; } = 0;
	public static int PredatorPopulation { get; set; } = 0;

	public static float FoodPercentage
	{
		get
		{
			return foodPercentage;
		}
		set
		{
			foodPercentage = (value / 10) * 1.5f;
		}
	}

	public static float OtherElementsPercentage
	{
		get
		{
			return otherElementsPercentage;
		}
		set
		{
			otherElementsPercentage = value / 10;
		}
	}
}
