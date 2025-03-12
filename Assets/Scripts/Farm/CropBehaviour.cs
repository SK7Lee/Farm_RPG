using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    SeedData seedToGrow;

    [Header("Stages of Life")]
    public GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;

    int growth;
    int maxGrowth;

    public enum CropState
    {
        Seed, Seedling, Harvestable
    }
    public CropState cropState;

    public void Plant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;
        seedling = Instantiate(seedToGrow.seedling, transform);
        ItemData cropToYield = seedToGrow.cropToYeild;
        harvestable = Instantiate(cropToYield.gameModel, transform);
        int hoursToGrow = GameTimestamp.DaysToHours(seedToGrow.daysToGrow);
        maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);
        SwitchState(CropState.Seed);
    }

    public void Grow()
    {
        growth++;
        if (growth >= maxGrowth / 2 && cropState == CropState.Seed) 
        { 
            SwitchState(CropState.Seedling);
        }

        if (growth >= maxGrowth && cropState == CropState.Seedling) 
        {
            SwitchState(CropState.Harvestable);
        }
    }

    void SwitchState(CropState stateToSwitch)
    {
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true); 
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);
                harvestable.transform.parent = null;
                Destroy(gameObject);
                break;
        }
        cropState = stateToSwitch;
    }
}
