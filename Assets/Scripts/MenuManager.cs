using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int startSceneIndex;
    public GameObject menuPanel;

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

    public void EndGame()
    {
        Application.Quit();
    }
}
