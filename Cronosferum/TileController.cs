using UnityEngine;

public class TileController : MonoBehaviour
{
	private AnimalManager animalManager;

	void OnMouseDown()
	{
		SpawnAnimal(AnimalController);
	}

	void OnMouseEnter()
	{
		//GetComponentInChildren<Renderer>().material.SetColor("_Color",Color.white);
	}

	void OnMouseExit()
	{
		//GetComponentInChildren<Renderer>().material.SetColor("_Color",Color.black);
	}

	private void Start()
	{
		
	}

	private void SpawnAnimal(GameObject animalBlueprint)
	{

	}
}
