using UnityEngine;
[System.Serializable]
public class WeatherProbability 
{
    public WeatherData.WeatherType weatherType;
    [Range(0f, 1f)]
    public float probability;
}
