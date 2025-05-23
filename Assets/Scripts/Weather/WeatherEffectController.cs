using UnityEngine;

public class WeatherEffectController : MonoBehaviour
{
    [SerializeField]
    GameObject rain, snow, heavySnow;
    Transform player;

    private void Start()
    {
        SceneTransitionManager.Instance.onLocationLoad.AddListener(LoadParticle);
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    private void FixedUpdate()
    {
        //move the particle to the player
        if (player != null)
        {
            Vector3 pos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime);
        }
    }

    void LoadParticle()
    {
        //disable everything
        rain.SetActive(false);
        snow.SetActive(false);
        heavySnow.SetActive(false);

        //move the particle to the player
        if (player != null)
        {
            Vector3 pos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.position = pos;
        }

        //check if indoor
        if (SceneTransitionManager.Instance.CurrentlyIndoor())
        {
            return;
        }
        if (WeatherManager.Instance.WeatherToday == WeatherData.WeatherType.Rain)
        {            
            rain.SetActive(true);
        }
        if (WeatherManager.Instance.WeatherToday == WeatherData.WeatherType.Snow)
        {
            snow.SetActive(true);
        }
        if (WeatherManager.Instance.WeatherToday == WeatherData.WeatherType.HeavySnow)
        {
            heavySnow.SetActive(true) ;
        }


    }

}
