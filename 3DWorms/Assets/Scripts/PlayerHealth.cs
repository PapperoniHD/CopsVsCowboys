using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    public TMP_Text hpText;
    public int hp;
    public bool isDead;
    void Start()
    {
        hp = 100;
        isDead = false;
    }
    void Update()
    {
        hpText.SetText(hp.ToString());
        
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;      
            this.gameObject.SetActive(false);
        }
    }
}
