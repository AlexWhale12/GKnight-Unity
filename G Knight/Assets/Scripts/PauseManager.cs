using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool GameIsPause;
    private GameObject stop;

    private void Start()
    {
        stop = GameObject.Find("Canvas1");
        stop.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                GameIsPause = false;
                stop.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                GameIsPause = true;
                stop.SetActive(true);
                Time.timeScale = 1f;
            }    
        }
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
