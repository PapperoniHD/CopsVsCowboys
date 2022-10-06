using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOfGame : MonoBehaviour
{
    public GameManager GM;
    public GameObject spinCam;
    public GameObject gameUI;
    public bool starting;
    public GameObject UI;
    public GameObject skipButton;
    public PauseManager pm;

    void Start()
    {
        Invoke("Starting", 15f);
        gameUI.SetActive(false);
        starting = true;
        GameManager.GM.StartOfRound();
        
    }

    public void Starting()
    {
        starting = false;
        //GM.enabled = true;
        gameUI.SetActive(true);
        spinCam.SetActive(false);
        UI.SetActive(false);
        GameManager.GM.Movement();
        Destroy(skipButton);
        pm.isPaused = false;
    }
}
