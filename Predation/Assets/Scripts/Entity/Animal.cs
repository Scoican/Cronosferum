using Predation.Managers;
using Predation.Map;
using Predation.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Predation.Entities
{
	public class Animal : Entity
	{
		[Header("Traits")]
		public Species foodPreference;
		public bool isMale;
		public float Speed;
		public float desirability;
		public float SensoryDistance;
		public int averageLifeSpan;
		public Genes Genes;

		[Header("State")]
		public float age;
		public float hunger;
		public float thirst;
		public float reproductionUrge;
		public EntityState currentState;

		[Header("Targets")]
		public Entity foodTarget;
		public Position WaterTarget;
		public Animal mateTarget;
		public Position LastWaterTarget;

		[Header("Color materials")]
		public Material maleMaterial;
		public Material femaleMaterial;

		private List<Animal> rejections;
		private Vector3 fullGrownSize;
		private float reproductionUrgeGrowRate = 100;
		private float criticalNeed = 0.75f;
		private float FleeginSpeed;
		private float InitialSpeed;

		private void Start()
		{
			fullGrownSize = Vector3.one;
			rejections = new List<Animal>();
			FleeginSpeed = Speed * 0.5f;
			InitialSpeed = Speed;
		}

		public void InitializeAnimal(bool recentlyBorn)
		{
			foodTarget = null;
			WaterTarget = Position.invalid;
			LastWaterTarget = Position.invalid;
			if (recentlyBorn)
			{
				InitializeChild();
			}
			else
			{
				InitializeAdult();
			}
			if (Genes != null)
			{
				if (isMale)
				{
					GetComponentInChildren<SkinnedMeshRenderer>().material = maleMaterial;
					desirability = Genes.GetGenderGene();
					GetComponentInChildren<SkinnedMeshRenderer>().material.color += new Color(maleMaterial.color.r * desirability, 0, 0);
				}
				else
				{
					GetComponentInChildren<SkinnedMeshRenderer>().material = femaleMaterial;
					reproductionUrge = 0;
				}
			}
		}

		public override void Die(CauseOfDeath causeOfDeath)
		{
			switch (species)
			{
				case Species.Rabbit:
					if (mateTarget != null)
					{
						mateTarget.mateTarget = null;
						mateTarget.currentState = EntityState.Wandering;
					}
					break;
				case Species.Wolf:
					if (mateTarget != null)
					{
						mateTarget.mateTarget = null;
						mateTarget.currentState = EntityState.Wandering;
					}
					if (foodTarget != null)
					{
						foodTarget.GetComponent<Animal>().currentState = EntityState.Wandering;
					}
					break;
			}
			base.Die(causeOfDeath);
		}

		public void SetGenes(Genes genes)
		{
			Genes = genes;
			isMale = Genes.isMale;
			if (isMale)
			{
				GetComponentInChildren<SkinnedMeshRenderer>().material = maleMaterial;
				desirability = Genes.GetGenderGene();
				reproductionUrge = 0;
				GetComponentInChildren<SkinnedMeshRenderer>().material.color += new Color(maleMaterial.color.r * desirability, 0, 0);
			}
			else
			{
				GetComponentInChildren<SkinnedMeshRenderer>().material = femaleMaterial;
				reproductionUrge = 0;
			}
			Speed *= Genes.GetSpeedGene();
			SensoryDistance *= Genes.GetSensoryDistanceGene();
		}

		private void InitializeAdult()
		{
			age = Random.Range(1, averageLifeSpan);
			Genes = Genes.RandomGenes();
			isMale = Genes.isMale;
		}

		private void InitializeChild()
		{
			age = 0;
			transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
			reproductionUrge = 0;
		}

		private void Update()
		{
			if (GameManager.gameState == GameStates.Running || GameManager.gameState == GameStates.Continued)
			{
				if (age >= 1 && GameManager.GameTimeSpeed > 1 && reproductionUrge < 1 && isMale)
				{
					reproductionUrge += Time.deltaTime * GameManager.GameTimeSpeed / reproductionUrgeGrowRate;
				}
				else
				{
					transform.localScale = Vector3.Slerp(transform.localScale, fullGrownSize, Time.deltaTime * GameManager.GameTimeSpeed / 10);
				}

				if (GameManager.GameTimeSpeed >= 1)
				{
					age += Time.deltaTime * GameManager.GameTimeSpeed / 100;
				}
				if (currentState == EntityState.Fleeing)
				{
					Speed = FleeginSpeed;
				}
				else
				{
					Speed = InitialSpeed;
				}

				if (currentState != EntityState.Fleeing && (hunger >= 0.40f || thirst >= 0.40f || reproductionUrge >= 0.25f))
				{
					SetNextAction();
				}

				if (hunger >= 1)
				{
					Die(CauseOfDeath.Hunger);
				}
				if (thirst >= 1)
				{
					Die(CauseOfDeath.Thirst);
				}
				if (age >= averageLifeSpan && Random.Range(0f, 1f) <= 0.33f)
				{
					Die(CauseOfDeath.Age);
				}
				if (isMale && reproductionUrge >= criticalNeed)
				{
					rejections.Clear();
				}
			}
		}

		private void SetNextAction()
		{
			if (hunger >= criticalNeed || thirst >= criticalNeed)
			{
				if (hunger > thirst)
				{
					currentState = EntityState.SatisfyHunger;
				}
				else
				{
					currentState = EntityState.SatisfyThirst;
				}
			}
			else
			{
				if (reproductionUrge >= 0.25f && isMale)
				{
					currentState = EntityState.LookingForMate;
				}
				else
				{
					if (hunger > thirst)
					{
						currentState = EntityState.SatisfyHunger;
					}
					else
					{
						currentState = EntityState.SatisfyThirst;
					}
				}
			}
		}

		public Entity SenseFood(Vector3 center)
		{
			Collider[] hitColliders = Physics.OverlapSphere(center, SensoryDistance);
			var closeEnties = new List<Entity>();
			foreach (var objectCollided in hitColliders)
			{
				if (objectCollided.gameObject.layer == LayerMask.NameToLayer("Entity"))
				{
					if (objectCollided.GetComponent<Entity>().species == foodPreference)
					{
						closeEnties.Add(objectCollided.GetComponent<Entity>());
					}
				}
			}
			if (closeEnties.Count != 0)
			{
				return GetClosestEntity(closeEnties);
			}
			else
			{
				return null;
			}
		}

		public Position SenseWater(Vector3 center)
		{
			Collider[] hitColliders = Physics.OverlapSphere(center, SensoryDistance);
			var closestWaterSource = new List<Position>();
			foreach (var objectCollided in hitColliders)
			{
				if (objectCollided.gameObject.layer == LayerMask.NameToLayer("Water"))
				{
					closestWaterSource.Add(objectCollided.GetComponent<Tile>().Position);
				}
			}
			if (closestWaterSource.Count != 0)
			{
				return GetClosestPosition(closestWaterSource);
			}
			else
			{
				return Position.invalid;
			}
		}

		public Animal SenseMate(Vector3 center)
		{
			Collider[] hitColliders = Physics.OverlapSphere(center, SensoryDistance);
			var closeEnties = new List<Entity>();
			foreach (var objectCollided in hitColliders)
			{
				if (objectCollided.gameObject.layer == LayerMask.NameToLayer("Entity"))
				{
					if (objectCollided.GetComponent<Entity>().species == species)
					{
						var entityCollided = objectCollided.GetComponent<Animal>();
						if (!entityCollided.isMale && entityCollided.mateTarget == null && !rejections.Contains(entityCollided) && entityCollided.Id != Id)
						{
							closeEnties.Add(objectCollided.GetComponent<Entity>());
						}
					}
				}
			}
			if (closeEnties.Count > 1)
			{
				var potentialMateEntity = GetClosestEntity(closeEnties);
				var potentialMateAnimal = potentialMateEntity.GetComponent<Animal>();
				if (PotentialMateFound(potentialMateAnimal))
				{
					potentialMateAnimal.currentState = EntityState.WaitingForMalePartener;
					return potentialMateAnimal;
				}
			}
			return null;
		}

		public bool PotentialMateFound(Animal femaleMate)
		{
			bool accepted = femaleMate.RequestMate(this);

			if (accepted)
			{
				mateTarget = femaleMate;
			}
			else
			{
				rejections.Add(femaleMate);
			}
			return accepted;
		}

		public bool RequestMate(Animal maleMate)
		{
			if (Random.Range(0f, 1f) > maleMate.desirability && age < 1)
			{
				return false;
			}
			mateTarget = maleMate;
			return true;
		}

		private Position GetClosestPosition(List<Position> entities)
		{
			var minValue = int.MaxValue;
			var closestEntityIndex = -1;
			for (int i = 0; i < entities.Count; i++)
			{
				var distance = Position.SqrDistance(position, entities[i]);
				if (distance < minValue)
				{
					minValue = distance;
					closestEntityIndex = i;
				}
			}
			return entities[closestEntityIndex];
		}

		private Entity GetClosestEntity(List<Entity> entities)
		{
			var minValue = int.MaxValue;
			var closestEntityIndex = -1;
			for (int i = 0; i < entities.Count; i++)
			{
				var distance = Position.SqrDistance(position, entities[i].position);
				if (distance < minValue)
				{
					minValue = distance;
					closestEntityIndex = i;
				}
			}
			if (closestEntityIndex != -1)
			{
				return entities[closestEntityIndex];
			}
			else
			{
				return null;
			}
		}
	}
}
