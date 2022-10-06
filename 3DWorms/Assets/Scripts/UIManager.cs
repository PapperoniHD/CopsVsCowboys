using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider teamCopHp;
    public Slider teamCowBoyHp;
    private void Start()
    {

        teamCopHp.maxValue = 200;
        teamCowBoyHp.maxValue = 200;

    }

    private void Update()
    {
        teamCopHp.value = GameManager.GM.teamCop[0].GetComponent<PlayerHealth>().hp +
                          GameManager.GM.teamCop[1].GetComponent<PlayerHealth>().hp;
        teamCowBoyHp.value = GameManager.GM.teamCowboy[0].GetComponent<PlayerHealth>().hp +
                          GameManager.GM.teamCowboy[1].GetComponent<PlayerHealth>().hp;
    }
}
