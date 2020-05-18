using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
	public MapManager Map;
	public MapGraph MapGraph;

	public Vector2 mapSize;

	static Dictionary<Species, List<Species>> preyBySpecies;
	static Dictionary<Species, List<Species>> predatorsBySpecies;

	private void Start()
	{
		InitializeEnvironment();
	}

	private void InitializeEnvironment()
	{
		
	}

	private void Awake()
	{
		Map.Size = mapSize;
	}
}
