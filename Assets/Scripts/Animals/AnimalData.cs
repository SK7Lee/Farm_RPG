using UnityEngine;

[CreateAssetMenu(menuName = "Animals/Animal")]
public class AnimalData : ScriptableObject
{
    public Sprite portrait;
    public AnimalBehaviour animalObject;

    //the price the player has to pay to purchase the animal
    public int purchasePrice;

    //the days it takes for the animal to mature
    public int daysToMature;

    // the item the animal produces
    public ItemData produce;

    //the location the animal spawns in
    public SceneTransitionManager.Location locationToSpawn;

}
