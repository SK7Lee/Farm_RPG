using UnityEngine;

[CreateAssetMenu(menuName ="Items/Seed")]
public class SeedData : ItemData
{
    public int daysToGrow;
    public ItemData cropToYeild;
    public GameObject seedling;
}
