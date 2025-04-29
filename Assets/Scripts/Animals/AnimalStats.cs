using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalStats : MonoBehaviour
{
    public const string ANIMAL_COUNT = "AnimalCount";
    public const string ANIMAL_DATA = "AnimalRelationship";

    /// This script is used to store the stats of animals
    public static List<AnimalRelationshipState> animalRelationships = new List<AnimalRelationshipState>();

    //Load all animal data from Resources folder
    static List<AnimalData> animals = Resources.LoadAll<AnimalData>("Animals").ToList();

    //To be fired up when animals born or purchased
    public static void StartAnimalCreation(AnimalData animalType)
    {
        if (animalType == null)
        {
            Debug.LogError("StartAnimalCreation was called with null animalType!");
            return;
        }
        //retrieve blackboard
        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();
        //initialise animal count parameter
        if (!blackboard.TryGetValue(ANIMAL_COUNT, out int animalCount))
        {
            blackboard.SetValue(ANIMAL_COUNT, 0);
            animalCount = 0;
        }
        //handle stats on animal type
        if (!blackboard.TryGetValue(ANIMAL_COUNT + animalType.name, out int animalTypeCount))
        {
            blackboard.SetValue(ANIMAL_COUNT + animalType.name, 0);
            animalTypeCount = 0;
        }

        //handle animal spawn
        UIManager.Instance.TriggerNamingPrompt($"Give your new {animalType.name} a name", (inputString) =>
        {
            //create a new animal and add it to the list of animals
            AnimalRelationshipState animalRelationshipData = new AnimalRelationshipState(animalCount, inputString, animalType); // Set default mood to max
            //animal entries are set by name
            blackboard.SetValue(ANIMAL_DATA + animalCount, animalRelationshipData);

            //statistics of the animal
            blackboard.SetValue(ANIMAL_COUNT, ++animalCount);
            blackboard.SetValue(ANIMAL_COUNT + animalType.name, ++animalTypeCount);

            //add it to the list of animals
            animalRelationships.Add(animalRelationshipData);
        });
    }
    //Load the animal data from the save file
    public static void LoadStats()
    {
        //load from the blackboard data
        animalRelationships = new List<AnimalRelationshipState>();
        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();

        //get the animal count from the blackboard
        if (blackboard.TryGetValue(ANIMAL_COUNT, out int animalCount))
        {
            for (int i = 0; i < animalCount; i++)
            {
                if (!blackboard.TryGetValue(ANIMAL_DATA + i, out AnimalRelationshipState animalRelationship)) continue;
                animalRelationships.Add(animalRelationship);
                
            }
        }

        /*
        if (relationshipsToLoad == null)
        {
            animalRelationships = new List<AnimalRelationshipState>();
            return;
        }
        animalRelationships = relationshipsToLoad;
        */
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
        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();
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
            animal.givenProduceToday = false;
            //advance the age of the animal
            animal.age += 1;
            //update its value in the blackboard
            blackboard.SetValue(ANIMAL_DATA + animal.id, animal);
        }
    }

    //Get the animal data from the name
    public static AnimalData GetAnimalTypeFromString(string name)
    {
        return animals.Find(i => i.name == name);
    }
}
