using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton;

    public void NewGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, null));
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, LoadGame));

    }

    void LoadGame()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.Log("Can't find Game State Manager");
            return;
        }
        GameStateManager.Instance.LoadSave();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFirstFrameLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        DontDestroyOnLoad(gameObject);
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading scene");
        }
        Debug.Log("Scene loaded");
        yield return new WaitForEndOfFrame();
        Debug.Log("First frame loaded");
        onFirstFrameLoad?.Invoke();

        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadGameButton.interactable = SaveManager.HasSave();
    }

}
