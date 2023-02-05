using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int startSceneIndex;
    public GameObject menuPanel;

    public GameObject victoryPanel;
    public GameObject losePanel;

    void Start()
    {
        if (Screen.fullScreen) {
            Screen.SetResolution(1920, 960, true); //1920x960 , 960x480
        } 
        else
        {
            Screen.SetResolution(960, 480, false); //1920x960 , 960x480
        }
        if (victoryPanel != null && losePanel != null) {
            if (PlayerPrefs.GetInt("victory") == 1)
            {
                victoryPanel.SetActive(true);
            } 
            else
            {
                losePanel.SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startSceneIndex, LoadSceneMode.Single);
    }

    public void BackToMenu(GameObject actualWindow)
    {
        actualWindow.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void GoToOther(GameObject openWindow)
    {
        menuPanel.SetActive(false);
        openWindow.SetActive(true);
    }

    public void EndScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
