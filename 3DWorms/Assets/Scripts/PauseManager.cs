using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseManager : StartOfGame
{
    public GameObject inGameUI;
    public GameObject pauseUI;
    public bool isPaused;

    void Start()
    {
        pauseUI.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        Inputs();
        Pause();

    }


    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            isPaused = false;
        }

    }


    void Pause()
    {

        if (GameManager.GM.State != GameState.StartOfRound)
        {
            if (isPaused)
            {
                pauseUI.SetActive(true);
                inGameUI.SetActive(false);
                Time.timeScale = 0;


                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                pauseUI.SetActive(false);
                inGameUI.SetActive(true);
                Time.timeScale = 1;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            if (isPaused)
            {
                pauseUI.SetActive(true);
                Time.timeScale = 0;


                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                pauseUI.SetActive(false);
                Time.timeScale = 1;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        
        

        
    }

    public void Continue()
    {
        isPaused = false;
    }

    

    public void Return()
    {
        isPaused = false;
        SceneManager.LoadScene(0);
        
    }

}
