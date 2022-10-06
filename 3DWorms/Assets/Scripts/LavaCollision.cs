using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCollision : MonoBehaviour
{
    public Vector3 lavaKnockback;

    private void OnTriggerEnter(Collider other)
    {
        print("trigg");
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Lava!");
        if (collision.gameObject.tag == "Lava")
        {
            
            GetComponentInParent<PlayerHealth>().hp -= 10;
            GetComponentInParent<PlayerMovement>().velocity.y = 10;
        }
    }
}
