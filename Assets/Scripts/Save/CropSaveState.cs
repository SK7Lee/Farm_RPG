using UnityEngine;
using static CropBehaviour;

[System.Serializable]
public struct CropSaveState 
{
    public int landID;
    public string seedToGrow;
    public CropBehaviour.CropState cropState;
    public int health;
    public int growth;

    public CropSaveState(int landID, string seedToGrow, CropBehaviour.CropState cropState, int growth, int health)
    {
        this.landID = landID;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;

        this.health = health;
    }

    public void Grow()
    {
        growth++;

        SeedData seedInfo = (SeedData)InventoryManager.Instance.itemIndex.GetItemFromString(seedToGrow);
        int maxGrowth =  GameTimestamp.HoursToMinutes(GameTimestamp.DaysToHours(seedInfo.daysToGrow));
        int maxHealth = GameTimestamp.HoursToMinutes(48);

        if (health < maxHealth)
        {
            health++;
        }

        if (growth >= maxGrowth / 2 && cropState == CropBehaviour.CropState.Seed)
        {
            cropState = CropBehaviour.CropState.Seedling;
        }

        if (growth >= maxGrowth && cropState == CropBehaviour.CropState.Seedling)
        {
            cropState = CropBehaviour.CropState.Harvestable;
        }

    }

    public void Wither()
    {
        health--;
        if (health <= 0 && cropState != CropBehaviour.CropState.Seed)
        {
            cropState = CropBehaviour.CropState.Wilted;
            
        }

    }
}
