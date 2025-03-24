using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance { get; private set; }
    public List<StartPoint> startPoints;

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

    public Transform GetPlayerStartingPosition(SceneTransitionManager.Location enteringFrom)
    {
        StartPoint startingPoint = startPoints.Find(x => x.enteringFrom == enteringFrom);
        return startingPoint.playerStart;
    }

}
