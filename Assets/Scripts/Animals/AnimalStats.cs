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

    //Get the animal data from the name
    public static AnimalData GetAnimalTypeFromString(string name)
    {
        return animals.Find(x => x.name == name);
    }
}
