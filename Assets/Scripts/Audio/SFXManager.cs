using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    private AudioSource audioSource;

    [Header("Character Actions")]
    public AudioClip walkSFX;
    public AudioClip digSFX;
    public AudioClip chopSFX;
    public AudioClip mineSFX;
    public AudioClip plantSFX;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("SFXManager: AudioSource bị thiếu!");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayDig() => PlaySFX(digSFX);
    public void PlayChop() => PlaySFX(chopSFX);
    public void PlayMine() => PlaySFX(mineSFX);
    public void PlayPlant() => PlaySFX(plantSFX);

    // ----------- WALK LOOP FIX BELOW ---------------
    private bool isWalking = false;

    public void StartWalk()
    {
        if (!isWalking && walkSFX != null)
        {
            isWalking = true;
            audioSource.clip = walkSFX;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopWalk()
    {
        if (isWalking)
        {
            isWalking = false;
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }
    }
    // -----------------------------------------------

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            if (volume == 0f)
            {
                audioSource.Pause();
            }
        }
    }

    private bool wasSFXPlayingBeforePause = false;

    private void OnApplicationPause(bool pause)
    {
        HandleSFXPauseOrFocus(!pause);
    }

    private void OnApplicationFocus(bool focus)
    {
        HandleSFXPauseOrFocus(focus);
    }

    private void HandleSFXPauseOrFocus(bool isFocused)
    {
        if (isFocused)
        {
            if (wasSFXPlayingBeforePause && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                wasSFXPlayingBeforePause = true;
                audioSource.Pause();
            }
            else
            {
                wasSFXPlayingBeforePause = false;
            }
        }
    }

    public float GetVolume()
    {
        return audioSource != null ? audioSource.volume : 1f;
    }
}
