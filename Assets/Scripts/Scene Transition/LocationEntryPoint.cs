using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    private void OnTriggerEnter(Collider other)
    {
        //check if the collider belongs to the player
        if (other.tag == "Player")
        {
            //switch scenes to the location specified in the inspector
            SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);
        }

        //character walking to the location
        if (other.tag == "Item")
        {
            Destroy(other.gameObject);
        }
    }

}
