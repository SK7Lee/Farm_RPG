using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Internal Clock")]
    [SerializeField]
    GameTimestamp timestamp;
    public float timeScale = 1.0f;

    [Header("Day and Night cycle")]
    public Transform sunTransform;
    private float indoorAngle = 40;

    List<ITimeTracker> listeners = new List<ITimeTracker>();

    public bool TimeTicking { get; set; }
    private void Awake()
    {
        //If there is more than one instance of this class, destroy the new one
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the instance to this object
            Instance = this;
        }
    }

    private void Start()
    {
        timestamp = new GameTimestamp(0, GameTimestamp.Season.Spring, 1, 6, 0);
        TimeTicking = true;
        StartCoroutine(TimeUpdate());
    }

    public void LoadTime(GameTimestamp timestamp)
    {
        this.timestamp = new GameTimestamp(timestamp);
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            if (TimeTicking)
            {
                Tick();
            }
            yield return new WaitForSeconds(1 / timeScale);

        }
    }

    public void Tick()
    {
        timestamp.UpdateClock();

        //Inform the listeners
        foreach (ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }

        UpdateSunMovement();

    }

    public void SkipTime(GameTimestamp timeToSkipTo)
    {
        int timeToSkipInMinutes = GameTimestamp.TimestampToMinutes(timeToSkipTo);
        Debug.Log("Time to skip in minutes: " + timeToSkipInMinutes);
        int timeNowInMinutes = GameTimestamp.TimestampToMinutes(timestamp);
        Debug.Log("Time now in minutes: " + timeNowInMinutes);
        int differenceInMinutes = timeToSkipInMinutes - timeNowInMinutes;
        Debug.Log("Difference in minutes: " + differenceInMinutes);
        if (differenceInMinutes < 0)
        {
            Debug.LogError("Cannot skip to a time in the past");
            return;
        }
        for (int i = 0; i < differenceInMinutes; i++)
        {
            Tick();
        }
    }

    void UpdateSunMovement()
    {
        if (SceneTransitionManager.Instance.CurrentlyIndoor())
        {
            sunTransform.eulerAngles = new Vector3(indoorAngle, 0, 0);
            return;
        }

        int timeInMinutes = GameTimestamp.HoursToMinutes(timestamp.hour) + timestamp.minute;

        float sunAngle = .25f * timeInMinutes - 90;

        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    public GameTimestamp GetGameTimestamp()
    {
        return new GameTimestamp(timestamp);
    }


    //Handling listeners
    public void RegisterTracker(ITimeTracker listener)
    {

        listeners.Add(listener);
    }

    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }


}