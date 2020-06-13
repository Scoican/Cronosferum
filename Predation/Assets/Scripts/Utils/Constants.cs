using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Predation.Utils
{
	public class Constants : MonoBehaviour
	{
		public static IList<(int, int)> neighboursOffset = new ReadOnlyCollection<(int, int)>(new List<(int, int)> { (-1, 0), (0, -1), (0, 1), (1, 0) });
		public static Color32 WaterColorDark = new Color32(14, 80, 102, 255);
		public static Color32 WaterColorMediumDark = new Color32(19, 100, 128, 255);
		public static Color32 WaterColorMediumLight = new Color32(18, 120, 153, 255);
		public static Color32 WaterColorLight = new Color32(36, 150, 181, 255);

		public static Color32 HillColorDark = new Color32(71, 88, 41, 255);
		public static Color32 HillColorMediumDark = new Color32(95, 138, 66, 255);
		public static Color32 HillColorMediumLight = new Color32(134, 173, 89, 255);
		public static Color32 HillColorLight = new Color32(191, 177, 149, 255);

		public static Color32 MountainColorDark = new Color32(61, 48, 36, 255);
		public static Color32 MountainColorMediumDark = new Color32(115, 86, 50, 255);
		public static Color32 MountainColorMediumLight = new Color32(163, 133, 90, 255);

		public static string WOLF_BLUEPRINT = "wolf_blueprint";
		public static string CHICKEN_BLUEPRINT = "chicken_blueprint";
		public static string RABBIT_BLUEPRINT = "rabbot_blueprint";
		public static string PLANT_BLUEPRINT = "plant_blueprint";

		public static string ALL_ENTITIES = "All entities";
		public static string WOLVES = "Wolves";
		public static string RABBITS = "Rabbits";
		public static string PLANTS = "Plants";
	}
}
