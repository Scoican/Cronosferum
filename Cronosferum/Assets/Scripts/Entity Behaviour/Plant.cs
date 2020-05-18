using Predation.Utils;
using UnityEngine;

public class Plant : Entity
{
	const float consumeSpeed = 8;

	public float Consume(float amount)
	{
		float amountConsumed = Mathf.Max(0, Mathf.Min(AmountRemaining, amount));
		AmountRemaining -= amount * consumeSpeed;

		transform.localScale = Vector3.one * AmountRemaining;

		if (AmountRemaining <= 0)
		{
			Die(CauseOfDeath.Eaten);
		}

		return amountConsumed;
	}

	public float AmountRemaining { get; private set; } = 1;
}
