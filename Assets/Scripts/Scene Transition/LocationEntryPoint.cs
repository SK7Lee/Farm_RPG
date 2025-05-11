using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    [SerializeField]
    //for lock door
    bool locked = false;
    private void OnTriggerEnter(Collider other)
    {
        //check if the collider belongs to the player
        if (other.tag == "Player")
        {
            if (WeatherManager.Instance.WeatherToday == WeatherData.WeatherType.Typhoon && SceneTransitionManager.Instance.currentLocation == SceneTransitionManager.Location.PlayerHome || locked)
            {
                //you cant go out in a typhoon
                DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage("It's not safe to go out."));
                return;
            }

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
