using UnityEngine;

public class WeatherManager : MonoBehaviour, ITimeTracker
{
    public static WeatherManager Instance { get; private set; }
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

    public WeatherData.WeatherType WeatherToday { get; private set; }
    public WeatherData.WeatherType WeatherTomorrow { get; private set; }

    //check if the weather has been set before
    bool weatherSet = false;
    //the weather data
    [SerializeField] WeatherData weatherData;

    void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
    }

    public WeatherData.WeatherType ComputeWeather(GameTimestamp.Season season)
    {
        if (weatherData == null)
        {
            throw new System.Exception("No weather data loaded");
        }

        //what are the possible weathers to compute
        WeatherProbability[] weatherSet = null;

        switch (season)
        {
            case GameTimestamp.Season.Spring:
                weatherSet = weatherData.springWeather;
                break;
            case GameTimestamp.Season.Summer:
                weatherSet = weatherData.summerWeather;
                break;
            case GameTimestamp.Season.Autumn:
                weatherSet = weatherData.fallWeather;
                break;
            case GameTimestamp.Season.Winter:
                weatherSet = weatherData.winterWeather;
                break;
        }

        //roll a random value
        float randomValue = Random.Range(0, 1f);
        //initialise probability
        float culmProbability = 0;
        foreach (WeatherProbability weatherProbability in weatherSet)
        {
            culmProbability += weatherProbability.probability;
            //if in the probabilitu
            if (randomValue <= culmProbability)
            {
                return weatherProbability.weatherType;
            }
        }
        return WeatherData.WeatherType.Sunny;
    }

    public void LoadWeather(WeatherSaveState saveState)
    {
        weatherSet = true;
        WeatherToday = saveState.weather;

        //set the forecast
        WeatherTomorrow = ComputeWeather(TimeManager.Instance.GetGameTimestamp().season);
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //check if it is 6am
        if (timestamp.hour == 6 && timestamp.minute == 0)
        {
            //set the current weather
            if (!weatherSet)
            {
                WeatherToday = ComputeWeather(timestamp.season);
            }
            else
            {
                WeatherToday = WeatherTomorrow;
            }

            //Set the forecast
            WeatherTomorrow = ComputeWeather(timestamp.season);

            weatherSet = true;
            Debug.Log("The weather is " + WeatherToday.ToString());
        }
    }
}
