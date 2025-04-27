using UnityEngine;

public class ChickenBehaviour : AnimalBehaviour
{
    protected override void Start()
    {
        base.Start();
        LayEgg();
    }

    void LayEgg()
    {
        //check the age
        AnimalData animalType = AnimalStats.GetAnimalTypeFromString(relationship.animalType);

        //if not an adult, it cant lay eggs
        if (relationship.age < animalType.daysToMature)
        {
            return;
        }

        //as long as the chicken is not sad
        if (relationship.Mood > 20 && !relationship.givenProduceToday)
        {
            //lay an egg
            ItemData egg = InventoryManager.Instance.GetItemFromString("Egg");
            Instantiate(egg.gameModel, transform.position, Quaternion.identity);
            relationship.givenProduceToday = true;
        }
    }
}
