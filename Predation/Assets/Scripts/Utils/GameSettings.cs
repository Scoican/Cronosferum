namespace Predation.Utils
{
	public static class GameSettings
	{
		private static int mapSize = 20;

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
						mapSize = 20;
						break;
					case 2:
						mapSize = 30;
						break;
					case 3:
						mapSize = 40;
						break;
					default:
						mapSize = 20;
						break;
				}
			}
		}

		public static int PreyPopulation { get; set; } = 0;
		public static int PredatorPopulation { get; set; } = 0;
		public static float FoodPercentage { get; set; } = 0f;
		public static float OtherElementsPercentage { get; set; } = 0f;

		public static bool IsPopulationZero
		{
			get
			{
				return (PreyPopulation == 0 && PredatorPopulation == 0);
			}
			private set { }
		}
	}
}
