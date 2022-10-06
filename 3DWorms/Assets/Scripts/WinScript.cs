using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GM.teamCop[0].GetComponent<PlayerHealth>().hp +
                          GameManager.GM.teamCop[1].GetComponent<PlayerHealth>().hp <= 0)
        {

        }


        if (GameManager.GM.teamCowboy[0].GetComponent<PlayerHealth>().hp +
                          GameManager.GM.teamCowboy[1].GetComponent<PlayerHealth>().hp <= 0)
        {

        }
    }
}
