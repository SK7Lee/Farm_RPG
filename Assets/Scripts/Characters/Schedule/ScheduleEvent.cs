using UnityEngine;
[System.Serializable]
public struct ScheduleEvent 
{
    public string name;
    [Header("Conditions")]
    public GameTimestamp time;
    public GameTimestamp.DayOfTheWeek dayOfTheWeek;
    //so we can ovverride schedules for special occasions, etc
    public int priority;

    //by default, the schedule event is not active
    //check if the exact date is a condition
    public bool factorDate;
    public bool ignoreDayOfTheWeek;


    [Header("Position")]
    public SceneTransitionManager.Location location;
    public Vector3 coord;
    public Vector2 facing;
}
