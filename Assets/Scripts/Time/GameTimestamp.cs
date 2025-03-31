using UnityEngine;

[System.Serializable]
public class GameTimestamp
{
    public int year;
    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }
    public enum DayOfTheWeek
    {
        Saturday,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,

    }

    public Season season;
    public int day;
    public int hour;
    public int minute;

    //Constructor
    public GameTimestamp(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    //CReate a new instance of a GameTimestamp from another pre-existing one
    public GameTimestamp(GameTimestamp timestamp)
    {
        this.year = timestamp.year;
        this.season = timestamp.season;
        this.day = timestamp.day;
        this.hour = timestamp.hour;
        this.minute = timestamp.minute;
    }



    public void UpdateClock()
    {
        minute++;
        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }

        if (hour >= 24)
        {
            hour = 0;
            day++;
        }

        if (day > 30)
        {
            day = 1;
            if (season == Season.Winter)
            {
                season = Season.Spring;
                year++;
            }
            else
            {
                season++;
            }

        }
    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        int daysPassed = YearsToSeasons(year) + SeasonsToDays(season) + day;

        int dayIndex = daysPassed % 7;
        return (DayOfTheWeek)dayIndex;
    }

    public static int HoursToMinutes(int hours)
    {
        return hours * 60;
    }

    public static int DaysToHours(int days)
    {
        return days * 24;
    }

    public static int SeasonsToDays(Season seasons)
    {
        int seasonIndex = (int)seasons;
        return seasonIndex * 30;
    }

    public static int YearsToSeasons(int years)
    {
        return years * 4 * 30;
    }

    public static int YearsToDays(int years)
    {
        return years * 4 * 30;
    }

    public static int TimestampToMinutes(GameTimestamp timestamp)
    {
        return HoursToMinutes(DaysToHours(YearsToDays(timestamp.year)) + DaysToHours(SeasonsToDays(timestamp.season)) + DaysToHours(timestamp.day) + timestamp.hour) + timestamp.minute;
        
    }

    /// Calcilate the difference in time between two timestamps
    public static int CompareTimestamps(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {
        int timestamp1Hours = DaysToHours(YearsToDays(timestamp1.year)) + DaysToHours(SeasonsToDays(timestamp1.season)) + DaysToHours(timestamp1.day) + timestamp1.hour;
        int timestamp2Hours = DaysToHours(YearsToDays(timestamp2.year)) + DaysToHours(SeasonsToDays(timestamp2.season)) + DaysToHours(timestamp2.day) + timestamp2.hour;
        int difference = timestamp2Hours - timestamp1Hours;
        return Mathf.Abs(difference);
    }

}