using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class IncubationManager : MonoBehaviour
{
    public static List<EggIncubationSaveState> eggsIncubating = new List<EggIncubationSaveState>();
    public const int daysToIncubate = 3;

    public List<Incubator> incubators;
    public static UnityEvent onEggUpdate = new UnityEvent();

    private void OnEnable()
    {
        RegisterIncubators();
        LoadIncubatorData();
        onEggUpdate.AddListener(LoadIncubatorData);
    }

    private void OnDestroy()
    {
        onEggUpdate.RemoveListener(LoadIncubatorData);
    }


    public static void UpdateEggs()
    {
        if (eggsIncubating.Count == 0) return;
        foreach (EggIncubationSaveState egg in eggsIncubating.ToList())
        {
            egg.Tick();
            onEggUpdate?.Invoke();
            if (egg.timeToIncubate <= 0)
            {                
                eggsIncubating.Remove(egg);
                //Get the animal data from the incubator ID
                AnimalData chickenData = AnimalStats.GetAnimalTypeFromString("Chicken");
                //Handle the chick hatching here
                if (chickenData == null)
                {
                    Debug.LogError("Chicken AnimalData not found! Make sure there is a Chicken.asset in Resources/Animals");
                    return;
                }
                AnimalStats.StartAnimalCreation(chickenData);
            }
        }
    }
    void RegisterIncubators()
    { 
        for (int i = 0; i< incubators.Count; i++)
        {
            incubators[i].incubationID = i;
        }
    }

    void LoadIncubatorData()
    {
        if (eggsIncubating.Count == 0) return;
        foreach (EggIncubationSaveState egg in eggsIncubating)
        {
            bool isIncubating = true;
            Incubator incubatorToLoad = incubators[egg.incubatorID];
            if (egg.timeToIncubate <= 0)
            {
                isIncubating = false;

            }

            incubatorToLoad.SetIncubationState(isIncubating, egg.timeToIncubate);

        }

    }
}
