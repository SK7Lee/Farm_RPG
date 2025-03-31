using UnityEngine;
using static Land;

[System.Serializable]
public struct LandSaveState
{
    public Land.LandStatus landStatus;
    public GameTimestamp lastWatered;
    public Land.FarmObstacleStatus obstacleStatus;

    public LandSaveState(Land.LandStatus landStatus, GameTimestamp lastWatered,Land.FarmObstacleStatus obstacleStatus)
    {
        this.landStatus = landStatus;
        this.lastWatered = lastWatered;
        this.obstacleStatus = obstacleStatus;
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        if (landStatus == Land.LandStatus.Watered)
        {
            int hoursElapsed = GameTimestamp.CompareTimestamps(lastWatered, timestamp);
            Debug.Log("Hours Elapsed: " + hoursElapsed);

            if (hoursElapsed > 24)
            {
                landStatus = Land.LandStatus.Farmland;
            }
        }
        
    }
}
