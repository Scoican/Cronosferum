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
				entityManager.SelectEntityToSpawn(null);
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

	private void SpawnEntity(EntityBlueprint entityBlueprint)
	{
		var newEntity = Instantiate(entityBlueprint.entityPrefab, new Vector3(GetComponent<Tile>().MapPosition.x, GetComponent<Tile>().Height / 10, GetComponent<Tile>().MapPosition.y), Quaternion.identity);
		newEntity.gameObject.GetComponent<EntityController>().currentTile = tile;
		tile.Occupied = true;
		entityManager.Register(newEntity.GetComponent<Animal>());
	}

	private void Start()
	{
		entityManager = EntityManager.Instance;
		tile = GetComponent<Tile>();
		tileColor = GetComponentInChildren<Renderer>().material.color;
	}
}
