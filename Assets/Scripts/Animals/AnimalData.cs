using UnityEngine;

[CreateAssetMenu(menuName = "Animals/Animal")]
public class AnimalData : ScriptableObject
{
    public Sprite portrait;
    public AnimalBehaviour animalObject;
    public int purchasePrice;
    public ItemData produce;
    public SceneTransitionManager.Location locationToSpawn;

}
