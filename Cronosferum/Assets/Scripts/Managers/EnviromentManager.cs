using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Predation
{
	public class EnviromentManager : MonoBehaviour
	{
		public MapGraph MapGraph;
		public MapManager MapManager;

		private static EnviromentManager instance;

		public static EnviromentManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<EnviromentManager>();
					if (instance == null)
					{
						var container = new GameObject("EnviromentManager");
						instance = container.AddComponent<EnviromentManager>();
					}
				}
				return instance;
			}
		}
	}
}

