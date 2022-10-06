using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;


public enum GameState
{
    Movement,
    Weapon,
    Observing,
    Damage,
    Killing,
    Drop,
    EndOfRound,
    StartOfRound
}

public class GameManager : MonoBehaviour
{

    public bool teamCopActive;
    public bool teamCowboyActive;

    public bool roundEnd;

    public GameObject[] players;
    public GameObject[] teamCop;
    public GameObject[] teamCowboy;
    private int currentPlayerIndex;
    public bool isActive;
    public static GameManager GM;
    public GameState State { get; private set; }
    public float roundTimer;

    public float maxRoundTime = 20;
    
    //UI
    [Header("UI")] 
    public TMP_Text roundTimerText; 
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        teamCopActive = true;
        GM = this;
        State = GameState.Movement;
        roundTimer = maxRoundTime;
        currentPlayerIndex = 0;
        for (int i = 1; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerMovement>().playerActive = false;
            
        }

        if (players.Length > 0)
        {
            players[0].GetComponent<PlayerMovement>().playerActive = true;
        }



        
        

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GM.State != GameState.StartOfRound)
        {
            HandleRound();
        }

        

        print(State);
        
        
    }


    void HandleRound()
    {

        
        
        
        roundTimer -= Time.deltaTime;
        roundTimerText.SetText(roundTimer.ToString("F0"));

        if (players[currentPlayerIndex].GetComponent<PlayerHealth>().isDead == true)
        {
            print("A player died");
            roundTimer = 0;

            
        }

        




        if (roundTimer <= 0)
        {


            if (roundEnd)
            {
                
                roundTimer = 5;
                GM.State = GameState.EndOfRound;
                roundEnd = false;

            }
            else
            {

                
                if (GM.State == GameState.Movement)
                {
                    currentPlayerIndex++;
                    if (currentPlayerIndex < players.Length)
                    {
                        players[currentPlayerIndex - 1].GetComponent<PlayerMovement>().playerActive = false;
                        players[currentPlayerIndex].GetComponent<PlayerMovement>().playerActive = true;

                        players[currentPlayerIndex].GetComponent<WeaponScript>().grenadeAmmo = 2;
                        players[currentPlayerIndex].GetComponent<WeaponScript>().revolverAmmo = 1;
                    }
                    else
                    {
                        players[currentPlayerIndex - 1].GetComponent<PlayerMovement>().playerActive = false;
                        currentPlayerIndex = 0;
                        players[currentPlayerIndex].GetComponent<PlayerMovement>().playerActive = true;

                        players[currentPlayerIndex].GetComponent<WeaponScript>().grenadeAmmo = 2;
                        players[currentPlayerIndex].GetComponent<WeaponScript>().revolverAmmo = 1;
                    }

                    


                    roundTimer = maxRoundTime;
                }
                else
                {
                    roundTimer = 0;
                }


                if (GM.State == GameState.Movement)
                {
                    if (teamCopActive)
                    {
                        teamCopActive = false;
                        teamCowboyActive = true;
                    }
                    else
                    {
                        teamCopActive = true;
                        teamCowboyActive = false;
                    }
                }
                else
                {
                    //roundTimer = 0;
                }
            }




            



        }
    }

    void Switch()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && teamCopActive)
        {
            currentPlayerIndex++;
            if (currentPlayerIndex < players.Length)
            {
                teamCop[currentPlayerIndex - 1].GetComponent<PlayerMovement>().playerActive = false;
                teamCop[currentPlayerIndex].GetComponent<PlayerMovement>().playerActive = true;
            }
            else
            {
                teamCop[currentPlayerIndex - 1].GetComponent<PlayerMovement>().playerActive = false;
                currentPlayerIndex = 0;
                teamCop[currentPlayerIndex].GetComponent<PlayerMovement>().playerActive = true;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow) && teamCowboyActive)
        {
            currentPlayerIndex++;
            if (currentPlayerIndex < players.Length)
            {
                teamCowboy[currentPlayerIndex - 1].GetComponent<PlayerMovement>().playerActive = false;
                teamCowboy[currentPlayerIndex].GetComponent<PlayerMovement>().playerActive = true;
            }
            else
            {
                teamCowboy[currentPlayerIndex - 1].GetComponent<PlayerMovement>().playerActive = false;
                currentPlayerIndex = 0;
                teamCowboy[currentPlayerIndex].GetComponent<PlayerMovement>().playerActive = true;
            }
        }
        
    }
    


    public void Observing()
    {
        State = GameState.Observing;
    }

    public void Movement()
    {
        State = GameState.Movement;
    }
    
    public void Damage()
    {
        State = GameState.Damage;
    }

    public void Killing()
    {
        State = GameState.Killing;
    }

    public void StartOfRound()
    {
        State = GameState.StartOfRound;
    }

}
