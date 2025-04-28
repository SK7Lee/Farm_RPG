using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static SceneTransitionManager;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance { get; private set; }
    public List<StartPoint> startPoints;

    //the connections between locations
    private static readonly Dictionary<Location, List<Location>> sceneConnections = new Dictionary<Location, List<Location>>()
    {
        {Location.PlayerHome, new List<Location>{Location.Farm} },
        {Location.Farm, new List<Location>{Location.PlayerHome, Location.Town} } ,
        {Location.Town, new List<Location>{Location.Farm, Location.Forest, Location.YodelRanch} },
        {Location.Forest, new List<Location>{Location.Town} },
        {Location.YodelRanch, new List<Location>{Location.Town} },
    };
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

    public Transform GetExitPosition(Location exitingTo)
    {
        Transform startPoint = GetPlayerStartingPosition(exitingTo);
        return startPoint.parent.GetComponentInChildren<LocationEntryPoint>().transform;
    }

    //get the next scene in the traversal path
    public static Location GetNextLocation(Location currentScene, Location finalDestination)
    {
        //track visited locations
        Dictionary<Location, bool> visited = new Dictionary<Location, bool>();
        //store previous locations
        Dictionary<Location, Location> previousLocation = new Dictionary<Location, Location>();
        //queue for BFS traversal
        Queue<Location> worklist = new Queue<Location>();

        //mark the current scene as visited
        visited.Add(currentScene, false);
        worklist.Enqueue(currentScene);

        //bfs traversal
        while (worklist.Count > 0)
        {
            Location scene = worklist.Dequeue();
            if (scene == finalDestination)
            {
                //recontruct the path
                while (previousLocation.ContainsKey(scene) && previousLocation[scene] != currentScene)
                {
                    scene = previousLocation[scene];
                }
                return scene;
            }

            //enqueue possible destinations connected to the current scene
            if (sceneConnections.ContainsKey(scene))
            {
                List<Location> possibleDestinations = sceneConnections[scene];
                foreach (Location neighbor in possibleDestinations)
                {
                    if (!visited.ContainsKey(neighbor))
                    {
                        visited.Add(neighbor, false);
                        previousLocation.Add(neighbor, scene);
                        worklist.Enqueue(neighbor);
                    }
                }
            }
        }

        return currentScene;

    }
}
