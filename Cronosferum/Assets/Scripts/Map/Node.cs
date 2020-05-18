using Predation.Utils;

public class Node
{
	public Node(Position position)
	{
		this.position = position;
	}

	public Position position;
	public int gCost;
	public int hCost;
	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}

	public Node parent;
}