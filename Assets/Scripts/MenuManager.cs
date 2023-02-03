using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int startSceneIndex;

    public void StartGame()
    {
        SceneManager.LoadScene(startSceneIndex, LoadSceneMode.Single);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
