using Predation.Utils;
using UnityEngine;

public class Entity : MonoBehaviour
{
	public int Id = -1;
	public Species species;
	public Position position;

	protected bool IsDead;

	public virtual void Create(Position position)
	{
		this.position = position;
	}

	protected virtual void Die(CauseOfDeath causeOfDeath)
	{
		if (!IsDead)
		{
			IsDead = true;
			//Environment.RegisterDeath(this);
			Destroy(gameObject);
		}
	}
}
