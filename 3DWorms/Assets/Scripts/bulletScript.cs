using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("bullethit");

            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            health.hp -= 10;
        }
        Destroy(this.gameObject);
    }
}
