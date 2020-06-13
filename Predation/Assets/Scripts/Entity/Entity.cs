using Predation.Managers;
using Predation.Utils;
using UnityEngine;

namespace Predation.Entities
{
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

		public virtual void Die(CauseOfDeath causeOfDeath)
		{
			if (!IsDead)
			{
				IsDead = true;
				EntityManager.Instance.RegisterDeath(this, causeOfDeath);
				Destroy(gameObject);
			}
		}
	}
}
