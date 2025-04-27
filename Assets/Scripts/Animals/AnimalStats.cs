using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalStats : MonoBehaviour
{
    /// This script is used to store the stats of animals
    public static List<AnimalRelationshipState> animalRelationships = new List<AnimalRelationshipState>();

    //Load all animal data from Resources folder
    static List<AnimalData> animals = Resources.LoadAll<AnimalData>("Animals").ToList();

    //To be fired up when animals born or purchased
    public static void StartAnimalCreation(AnimalData animalType)
    {
        UIManager.Instance.TriggerNamingPrompt($"Give your new {animalType.name} a name", (inputString) =>
        {
            animalRelationships.Add(new AnimalRelationshipState(inputString, animalType));
        });
    }
    //Load the animal data from the save file
    public static void LoadStats(List<AnimalRelationshipState> relationshipsToLoad)
    {
        if (relationshipsToLoad == null)
        {
            animalRelationships = new List<AnimalRelationshipState>();
            return;
        }
        animalRelationships = relationshipsToLoad;
    }

    //Get the animals by type
    public static List<AnimalRelationshipState> GetAnimalsByType(string animalTypeName)
    {
        return animalRelationships.FindAll(x => x.animalType == animalTypeName);
    }

    public static List<AnimalRelationshipState> GetAnimalsByType(AnimalData animalType)
    {
        return GetAnimalsByType(animalType.name);
    }

    public static void OnDayReset()
    {
        foreach (AnimalRelationshipState animal in AnimalStats.animalRelationships)
        {
            if (animal.hasTalkedToday)
            {
                animal.friendshipPoints += 30;
            }
            else
            {
                animal.friendshipPoints -= (10 - (animal.friendshipPoints / 200));
            }

            if (animal.giftGivenToday)
            {
                animal.Mood += 15;
            }
            else
            {
                animal.Mood -= 100;
                animal.friendshipPoints -= 20;
            }
            animal.hasTalkedToday = false;
            animal.giftGivenToday = false;

            //advance the age of the animal
            animal.age += 1;
        }
    }

    //Get the animal data from the name
    public static AnimalData GetAnimalTypeFromString(string name)
    {
        return animals.Find(x => x.name == name);
    }
}
