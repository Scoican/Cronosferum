using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Predation.Entities
{
	public class Genes
	{
		private const float MUTATION_CHANCE = .5f;
		private const float INHERITENCE_CHANCE = .5f;
		private const int NUMBER_OF_GENES = 4;

		public readonly bool isMale;
		public readonly List<float> values;

		public Genes(List<float> values, bool isMale)
		{
			this.isMale = isMale;
			this.values = values;
		}

		public static Genes RandomGenes()
		{
			List<float> values = new List<float>();
			values.AddRange(Enumerable.Repeat(0f, NUMBER_OF_GENES));
			values[0] = Random.Range(0f, 1f);
			values[1] = Random.Range(1f, 5f);
			values[2] = Random.Range(0f, 10f);
			values[3] = Random.Range(0f, 1f);
			return new Genes(values, Random.Range(0f, 1f) < 0.5f);
		}

		public static Genes InheritedGenes(Genes mother, Genes father)
		{
			var motherGenes = new List<float>(mother.values);
			var fatherGenes = new List<float>(father.values);
			var offspringGenes = new List<float>(mother.values.Count);
			offspringGenes.AddRange(Enumerable.Repeat(0f, NUMBER_OF_GENES));
			var isOffspringMale = Random.Range(0f, 1f) < 0.5f;

			//Gender specific gene
			if (Random.Range(0f, 1f) <= MUTATION_CHANCE)
			{
				offspringGenes[0] = Random.Range(0f, 1f);
			}
			else
			{
				offspringGenes[0] = isOffspringMale ? father.values[0] : mother.values[0];
			}

			//Parent inheritence genes
			for (var i = 1; i < offspringGenes.Count; i++)
			{
				offspringGenes[i] = (Random.Range(0f, 1f) < INHERITENCE_CHANCE) ? fatherGenes[i] : motherGenes[i];
				//offspringGenes[i] = System.Math.Max(fatherGenes[i], motherGenes[i]);
				if (Random.Range(0f, 1f) <= MUTATION_CHANCE)
				{
					offspringGenes[i] = Random.Range(0.5f, 1.5f);
				}
			}

			Genes genes = new Genes(offspringGenes, isOffspringMale);
			return genes;
		}

		public float GetGenderGene()
		{
			return values[0];
		}

		public float GetSpeedGene()
		{
			return values[1];
		}

		public float GetReproductionUrgeGene()
		{
			return values[2];
		}

		public float GetSensoryDistanceGene()
		{
			return values[3];
		}
	}
}