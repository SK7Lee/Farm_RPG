using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioSource;

    [Header("Music Settings")]
    public AudioClip[] musicTracks;         // Danh sách nhạc nền
    public float fadeDuration = 1f;         // Thời gian fade in/out
    private int currentTrackIndex = -1;
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
        audioSource.loop = false;
        audioSource.playOnAwake = false;

    }

    private void Start()
    {
        PlayRandomMusic();
    }

    private void Update()
    {
        // Nếu bài nhạc đã dừng → phát bài mới
        if (audioSource.volume == 0f)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            return; // Dừng kiểm tra và không chuyển bài mới nếu volume = 0
        }

        // Nếu bài nhạc đã dừng → phát bài mới
        if (!audioSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }
    public int GetCurrentMusicIndex() => currentTrackIndex;

    public void SetCurrentMusicIndex(int index)
    {
        if (index >= 0 && index < musicTracks.Length)
        {
            currentTrackIndex = index;
            AudioClip newClip = musicTracks[index];
            StartCoroutine(SwitchMusic(newClip));
        }
        else
        {
            Debug.LogWarning("Invalid music track index: " + index);
        }
    }

    public void PlayRandomMusic()
    {
        if (audioSource.volume == 0f)
        {
            return;
        }

        if (wasPlayingBeforePause && currentClip != null)
        {
            audioSource.clip = currentClip;  // Đảm bảo phát bài cũ
            audioSource.Play();
            return;  // Dừng lại ở đây nếu không muốn chuyển bài
        }
        if (musicTracks.Length == 0) return;

        currentTrackIndex = Random.Range(0, musicTracks.Length);
        AudioClip newClip = musicTracks[currentTrackIndex];

        StartCoroutine(SwitchMusic(newClip));
    }

    private bool wasPlayingBeforePause = false;
    private AudioClip currentClip = null;
    private void OnApplicationPause(bool pause)
    {
        HandleAppPauseOrFocus(!pause);
    }

    private void OnApplicationFocus(bool focus)
    {
        HandleAppPauseOrFocus(focus);
    }

    private void HandleAppPauseOrFocus(bool isFocused)
    {
        if (isFocused)
        {
            // Khi quay lại và nhạc đang chơi, tiếp tục phát nhạc
            if (wasPlayingBeforePause && audioSource.clip != null)
            {
                audioSource.Play();
                audioSource.volume = 0.5f;  // Đảm bảo volume khi quay lại
            }
        }
        else
        {
            // Khi tạm dừng, tạm dừng nhạc
            if (audioSource.isPlaying)
            {
                wasPlayingBeforePause = true;
                currentClip = audioSource.clip;  // Lưu bài nhạc hiện tại
                audioSource.Pause();
            }
            else
            {
                wasPlayingBeforePause = false;
            }
        }
    }
    private IEnumerator SwitchMusic(AudioClip newClip)
    {
        if (audioSource.volume == 0f)
        {
            yield break;
        }
        float originalVolume = audioSource.volume;
        // Fade out if there's an active track
        // Fade out
        for (float vol = originalVolume; vol > 0f; vol -= Time.deltaTime / fadeDuration)
        {
            audioSource.volume = vol;
            yield return null;
        }
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in the new track
        for (float vol = 0f; vol < originalVolume; vol += Time.deltaTime / fadeDuration)
        {
            audioSource.volume = vol;
            yield return null;
        }

        audioSource.volume = originalVolume;  // Ensure the volume is set to a reasonable level.
    }


    public void SetVolume(float volume)
    {
        audioSource.volume = volume;

        if (Mathf.Approximately(volume, 0f))
        {
            // Nếu volume = 0, tạm dừng nhạc
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                wasPlayingBeforePause = true;
                currentClip = audioSource.clip;
            }
            else
            {
                wasPlayingBeforePause = false;
            }
        }
        else
        {
            // Volume > 0
            if (!audioSource.isPlaying && wasPlayingBeforePause && audioSource.clip != null)
            {
                audioSource.UnPause(); // ✅ tiếp tục thay vì phát lại từ đầu
            }

            wasPlayingBeforePause = false;
        }
    }

    public float GetVolume()
    {
        return audioSource != null ? audioSource.volume : 1f;
    }

}
