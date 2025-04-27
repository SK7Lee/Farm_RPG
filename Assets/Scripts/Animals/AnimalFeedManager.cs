using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalFeedManager : MonoBehaviour
{
    //All the feedboxes in the scene
    public static Dictionary<AnimalData, bool[]> feedboxStatus = new Dictionary<AnimalData, bool[]>();

    public Feedbox[] feedboxes;
    //the associated animal data
    public AnimalData animal;

    private void OnEnable()
    {
        feedboxes = GetComponentsInChildren<Feedbox>();
        RegisterFeedboxes();
        LoadFeedboxData();
    }

    public static void ResetFeedboxes()
    {
        feedboxStatus = new Dictionary<AnimalData, bool[]>();
    }

    public void FeedAnimal(int id)
    {
        //get the list of animals that are eligible to be fed
        List<AnimalRelationshipState> eligibleAnimals = AnimalStats.GetAnimalsByType(animal);

        //find the first animal that is not fed
        foreach (AnimalRelationshipState a in eligibleAnimals)
        {
            if (!a.giftGivenToday)
            {
                a.giftGivenToday = true;
                Debug.Log($"Fed {a.name} with {animal.name}");
                break;
            }
        }
        //update the feedbox status
        feedboxStatus[animal][id] = true;
        //make sure the feedbox is set to true
        LoadFeedboxData();

    }

    //assign feedboxes an id
    void RegisterFeedboxes()
    {
        for (int i = 0; i < feedboxes.Length; i++)
        {
            feedboxes[i].id = i;
        }
    }

    void LoadFeedboxData()
    {
        //check if feedbox status is already loaded
        if (!feedboxStatus.ContainsKey(animal))
        {
            //if not, create a new array of bools
            feedboxStatus.Add(animal, new bool[feedboxes.Length]);
        }

        //get the current feedbox status
        bool[] currentFeedboxStatus = feedboxStatus[animal];
        //set the feedbox status
        for (int i = 0; i < feedboxes.Length; i++)
        {
            //set the feedbox status
            feedboxes[i].SetFeedState(currentFeedboxStatus[i]);
        }
    }

}
