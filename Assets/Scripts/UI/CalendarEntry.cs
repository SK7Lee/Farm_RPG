using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class CalendarEntry : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI dateText;
    [SerializeField]
    Image icon;
    //The colour of the entry
    Image entry;
    //The colours of the day
    [SerializeField]
    Color weekday, sat, sun, today;
    public GameTimestamp.Season season;
    string eventDescription;

    // Start is called before the first frame update
    void OnEnable()
    {
        icon.gameObject.SetActive(false);
        entry = GetComponent<Image>();
    }

    //For days with special events
    public void Display(int date, GameTimestamp.DayOfTheWeek day, Sprite eventSprite, string eventDescription, bool isToday)
    {
        dateText.text = date.ToString();
        Color colorToSet = weekday;    
        if (isToday)
        {
            colorToSet = today;
        }
        else
        {
            switch (day)
            {
                case GameTimestamp.DayOfTheWeek.Saturday:
                    colorToSet = sat;
                    break;
                case GameTimestamp.DayOfTheWeek.Sunday:
                    colorToSet = sun;
                    break;
                default:
                    colorToSet = weekday;
                    break;
            }
        }
            

        entry.color = colorToSet;

        if (eventSprite != null)
        {
            icon.sprite = eventSprite;
            icon.gameObject.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(false);
        }

    }

    //For normal days
    public void Display(int date, GameTimestamp.DayOfTheWeek day, bool isToday)
    {

        Display(date, day, null, "Just an ordinary day", isToday);

    }

    //For null entries
    public void EmptyEntry()
    {
        entry.color = Color.clear;
        dateText.text = "";
        icon.gameObject.SetActive(false);
    }
}