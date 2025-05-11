using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject settingsScreen;
    public GameObject guideScreen;

    public void OpenMenu()
    {
        menuScreen.SetActive(true);
    }

    public void CloseMenu()
    {
        menuScreen.SetActive(false);
    }

    public void OpenSettings()
    {
        Debug.Log("Mở cài đặt...");
        
        menuScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void ShowGuide()
    {
        Debug.Log("Mở hướng dẫn...");
        menuScreen.SetActive(false);
        guideScreen.SetActive(true);
    }

    public void CloseGuide()
    {
        guideScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        GameObject target = GameObject.Find("Essentials");
        if (target != null)
        {
            Destroy(target);
        }
        Debug.Log("Về màn hình chính...");
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        Debug.Log("Thoát game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
