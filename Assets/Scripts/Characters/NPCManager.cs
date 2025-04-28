using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NPCManager : MonoBehaviour, ITimeTracker
{
    public static NPCManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    List<CharacterData> characters = null;

    List<NPCScheduleData> npcSchedules;

    [SerializeField]
    List<NPCLocationState> npcLocations;

    //load all character data
    public List<CharacterData> Characters()
    {
        if (characters != null) return characters;
        CharacterData[] characterDatabase = Resources.LoadAll<CharacterData>("Characters");
        characters = characterDatabase.ToList();
        return characters;
    }

    private void OnEnable()
    {
        //load npc schedules
        NPCScheduleData[] schedules = Resources.LoadAll<NPCScheduleData>("Schedules");
        npcSchedules = schedules.ToList();
        InitNPCLocations();
    }

    private void Start()
    {
        //add this to TimeManager to update NPCs
        TimeManager.Instance.RegisterTracker(this);
        SceneTransitionManager.Instance.onLocationLoad.AddListener(RenderNPCs);
    }

    private void InitNPCLocations()
    {
        npcLocations = new List<NPCLocationState>();
        foreach (CharacterData character in Characters())
        {
            npcLocations.Add(new NPCLocationState(character));
        }
    }

    void RenderNPCs()
    {
        foreach (NPCLocationState npc in npcLocations)
        {
            if (npc.location == SceneTransitionManager.Instance.currentLocation)
            {
                Instantiate(npc.character.prefab, npc.coord, Quaternion.Euler(npc.facing));
            }
        }
    }

    void SpawnInNPC(CharacterData npc, SceneTransitionManager.Location comingFrom)
    {
        Transform start = LocationManager.Instance.GetPlayerStartingPosition(comingFrom);
        Instantiate(npc.prefab, start.position, start.rotation);
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        UpdateNPCLocations(timestamp);
    }

    public NPCLocationState GetNPCLocation(string name)
    {
        return npcLocations.Find(x => x.character.name == name);
    }
    private void UpdateNPCLocations(GameTimestamp timestamp)
    {
        for (int i = 0; i < npcLocations.Count; i++)
        {
            NPCLocationState npcLocator = npcLocations[i];
            SceneTransitionManager.Location previousLocation = npcLocator.location;
            //find the schedule for the character
            NPCScheduleData schedule = npcSchedules.Find(x => x.character == npcLocator.character);
            if (schedule == null)
            {
                Debug.LogError($"No schedule found for {npcLocator.character.name}");
                continue;
            }

            //current time
            GameTimestamp.DayOfTheWeek dayOfWeek = timestamp.GetDayOfTheWeek();

            //find the events that correspond to the current time
            //either the day of the week matches or the time matches
            List<ScheduleEvent> eventsToConsider = schedule.npcScheduleList.FindAll(x => x.time.hour <= timestamp.hour && (x.dayOfTheWeek == dayOfWeek || x.ignoreDayOfTheWeek));
            //check if the events are empty
            if (eventsToConsider.Count < 1)
            {
                Debug.LogError("None found for " + npcLocator.character.name);
                Debug.LogError(timestamp.hour);
                continue;
            }

            //remove all events with the hour that is lower than the max time
            int maxHour = eventsToConsider.Max(x => x.time.hour);
            eventsToConsider.RemoveAll(x => x.time.hour < maxHour);

            // get the event with the highest priority
            ScheduleEvent eventToExecute = eventsToConsider.OrderByDescending(x => x.priority).First();
            Debug.Log(eventToExecute.name); // Fix: Access the 'name' property of the selected ScheduleEvent instead of the list
            //set the npc locator value accordingly
            npcLocations[i] = new NPCLocationState(schedule.character, eventToExecute.location, eventToExecute.coord, eventToExecute.facing);
            SceneTransitionManager.Location newLocation = eventToExecute.location;
            //if there has been a change in location
            if (newLocation != previousLocation)
            {
                Debug.Log("New location: " + newLocation);
                //if the location is where we are
                if (SceneTransitionManager.Instance.currentLocation == newLocation)
                {
                    SpawnInNPC(schedule.character, previousLocation);
                }
            }
        }
    }

}
