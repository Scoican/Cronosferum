using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;

public class MapGraph : MonoBehaviour
{
	private Dictionary<Position, Node> Nodes = new Dictionary<Position, Node>();

	public void GenerateGraph(MapManager map)
	{
		Nodes.Clear();
		for (int i = 0; i < map.Size.x; i++)
		{
			for (int j = 0; j < map.Size.y; j++)
			{
				var tile = map.GetTile(new Position(i, j));
				Nodes.Add(tile.MapPosition, new Node(new Position(i, j)));
			}
		}
	}

	public Node GetNode(int x, int y)
	{
		if (Nodes.ContainsKey(new Position(x, y)))
			return Nodes[new Position(x, y)];
		return null;
	}

	public void AddNode(int x, int y)
	{
		if (Nodes.ContainsKey(new Position(x, y)))
			return;
		Nodes.Add(new Position(x, y), new Node(new Position(x, y)));
	}

	public bool RemoveNode(int x, int y)
	{
		return Nodes.Remove(new Position(x, y));
	}

	public List<Node> GetNodeNeighbours(Node node)
	{
		if (node == null)
		{
			Debug.LogError("Can not get neighbours for null node!!!");
			return null;
		}
		var result = new List<Node>();
		foreach (var neighbour in Constants.neighboursOffset)
		{
			var neighbourNode = GetNode(node.position.x + neighbour.Item1, node.position.y + neighbour.Item2);
			if (neighbourNode != null)
				result.Add(neighbourNode);
		}
		return result;
	}

	public List<Node> AStarSearch(Position source, Position destination)
	{
		var startNode = GetNode(source.x, source.y);
		var endNode = GetNode(destination.x, destination.y);

		var openNodes = new List<Node>();
		var closedNodes = new HashSet<Node>();
		openNodes.Add(startNode);

		if (startNode == null || endNode == null)
		{
			Debug.LogError("Invalid start or end node!");
			return null;
		}

		while (openNodes.Count > 0)
		{
			var node = openNodes[0];
			for (int i = 1; i < openNodes.Count; i++)
			{
				if (openNodes[i].fCost < node.fCost || openNodes[i].fCost == node.fCost)
				{
					if (openNodes[i].hCost < node.hCost)
						node = openNodes[i];
				}
			}
			openNodes.Remove(node);
			closedNodes.Add(node);

			if (node == endNode)
			{
				return RetracePath(startNode, endNode);
			}

			foreach (Node neighbour in GetNodeNeighbours(node))
			{
				if (closedNodes.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + (int)Position.Distance(node.position, neighbour.position);
				if (newCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = (int)Position.Distance(neighbour.position, endNode.position);
					neighbour.parent = node;

					if (!openNodes.Contains(neighbour))
						openNodes.Add(neighbour);
				}
			}
		}
		return null;
	}

	public List<Node> RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
		return path;
	}
}
