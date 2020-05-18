using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapBuilder))]
public class TerrainGeneratorEditor : Editor
{
	MapBuilder terrainGen;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Refresh"))
		{
			terrainGen.gameObject.GetComponent<MapManager>().RegenerateMap();
		}
	}

	void OnEnable()
	{
		terrainGen = (MapBuilder)target;
	}
}