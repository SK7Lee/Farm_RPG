using UnityEngine;
using System.Collections.Generic;

public class CalenderInteractable : InteractableObject
{
    public override void Pickup()
    {
        CalendarUIListing calendar = UIManager.Instance.calendar;
        calendar.gameObject.SetActive(true);
        calendar.RenderCalendar(TimeManager.Instance.GetGameTimestamp());
    }
}
