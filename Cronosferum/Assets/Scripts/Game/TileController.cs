using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour
{
	private EntityManager entityManager;
	private Tile tile;
	private Color tileColor;

	void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		if (entityManager.GetEntityToSpawn() != null)
		{
			if (!tile.Occupied && tile.Type != Tile.TileType.Water)
			{
				SpawnEntity(entityManager.GetEntityToSpawn());
			}
			else
			{
				Debug.Log("CAN'T PLACE HERE!");
			}
		}
		else
		{
			Debug.Log("PLEASE SELECT AN ANIMAL!");
		}
		
	}

	private void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(1))
		{
			Debug.Log($"This tile is {tile.Type} and has a position of {tile.Position}");
		}
	}

	void OnMouseEnter()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		GetComponentInChildren<Renderer>().material.SetColor("_Color", tileColor + tileColor / 2);
	}

	void OnMouseExit()
	{
		GetComponentInChildren<Renderer>().material.SetColor("_Color", tileColor);
	}

	public void SpawnEntity(EntityBlueprint entityBlueprint)
	{
		if (tile == null)
		{
			tile = GetComponent<Tile>();
		}
		var newEntity = Instantiate(entityBlueprint.entityPrefab, new Vector3(GetComponent<Tile>().Position.x, GetComponent<Tile>().Height / 10, GetComponent<Tile>().Position.y), Quaternion.identity);
		newEntity.gameObject.GetComponent<BaseAnimalController>().currentTile = tile;
		tile.Occupied = true;
		EntityManager.Instance.Register(newEntity.GetComponent<Animal>());
	}

	private void Start()
	{
		entityManager = EntityManager.Instance;
		tile = GetComponent<Tile>();
		tileColor = GetComponentInChildren<Renderer>().material.color;
	}
}
