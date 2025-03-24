using UnityEngine;

[System.Serializable]
public class CropSaveState : MonoBehaviour
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
}
