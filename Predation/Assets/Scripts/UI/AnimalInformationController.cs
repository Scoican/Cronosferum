using UnityEngine;
using Predation.Utils;
using Predation.Entities;

namespace Predation.UI
{
	public class AnimalInformationController : MonoBehaviour
	{
		public AnimalInformationView View;

		public Sprite WolfImage;
		public Sprite RabbitImage;

		public void DisplayAnimalInformation(Animal animal)
		{
			if (animal.species == Species.Wolf)
			{
				View.AnimalImage.sprite = WolfImage;
			}
			else
			{
				View.AnimalImage.sprite = RabbitImage;
			}

			if (animal.isMale)
			{
				View.GenderText.text = "Gender: Male";
			}
			else
			{
				View.GenderText.text = "Gender: Female";
			}

			View.SpeciesText.text = $"Species: {animal.species}";
			View.SpeedText.text = $"Speed: {animal.Speed.ToString("F2")}";
			View.AgeText.text = $"Age: {animal.age.ToString("F1")}";
			View.DesirabilityText.text = $"Desirability: {(int)(animal.desirability * 100)}%";
			View.ViewRangeText.text = $"View Range: {animal.SensoryDistance.ToString("F2")}";
			View.HungerText.text = $"Hunger: {(int)(animal.hunger * 100)}%";
			View.ThristText.text = $"Thirst: {(int)(animal.thirst * 100)}%";
			View.ReproductionUrgeText.text = $"Rep. Urge: {(int)(animal.reproductionUrge * 100)}%";
		}
	}
}
