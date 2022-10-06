using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonUI : MonoBehaviour
{
    public GameObject tutorialCam;

    private void Start()
    {
        tutorialCam.SetActive(false);
    }

    public void NewGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void TutorialOn()
    {
        tutorialCam.SetActive(true);
    }

    public void TutorialOff()
    {
        tutorialCam.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
