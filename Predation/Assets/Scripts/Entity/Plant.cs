using Predation.Managers;
using Predation.Utils;
using UnityEngine;

namespace Predation.Entities
{
	public class Plant : Entity
	{
		private const float consumeSpeed = 8;

		private bool shouldGrow;

		public bool ShouldGrow
		{
			get
			{
				return shouldGrow;
			}
			set
			{
				if (value)
				{
					transform.localScale = new Vector3(-1f, -1f, -1f);
				}
				shouldGrow = value;
			}
		}

		public bool IsGrowing;

		private void Update()
		{
			if (GameManager.gameState == GameStates.Running || GameManager.gameState == GameStates.Continued)
			{
				if (ShouldGrow)
				{
					transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one, Time.deltaTime * GameManager.GameTimeSpeed / 10);
				}
				IsGrowing = transform.localScale.x > 0.25f;
				if (IsGrowing && AmountRemaining > 0)
				{
					AmountRemaining = transform.localScale.x;
				}
			}
		}

		public float Consume(float amount)
		{
			float amountConsumed = Mathf.Max(0, Mathf.Min(AmountRemaining, amount));
			AmountRemaining -= Mathf.Abs(amount * consumeSpeed);

			transform.localScale = Vector3.one * AmountRemaining;

			if (AmountRemaining <= 0)
			{
				Die(CauseOfDeath.Eaten);
			}

			return amountConsumed;
		}

		public float AmountRemaining { get; private set; } = 1;
	}
}