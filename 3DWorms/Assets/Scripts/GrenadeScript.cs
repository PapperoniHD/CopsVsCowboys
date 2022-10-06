using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GrenadeScript : GameManager
{
    //GameObjects
    public ParticleSystem explosion;
    public GameObject mesh;
    public float timer = 0f;
    public bool isPlayed = false;
    AudioSource explode;

    //Variables
    public float radius = 5f;
    public Transform grenadeTransform;
    public float explosionForce;
    public float dist;
    public Vector3 dir;

    void Update()
    {
        //Timer
        timer += Time.deltaTime;
        if (timer <= 5f)
        {
            mesh.SetActive(true);
        }
        else
        {
            if (!isPlayed)
            {          
                Instantiate(explosion, transform.position, transform.rotation);
                isPlayed = true;                 
                Explode();
            }
            mesh.SetActive(false);
        }

        if (timer >= 7)
        {
            GameManager.GM.Movement();
            Destroy(this.gameObject);
        }

        //Play Sound Effect
        explode = GameObject.Find("ExplodeSound").GetComponent<AudioSource>();
        
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        explode.Play();
        foreach (Collider nearbyObject in colliders)
        {
            PlayerHealth health = nearbyObject.GetComponent<PlayerHealth>();
            PlayerMovement velocity = nearbyObject.GetComponent<PlayerMovement>();
            if (nearbyObject.CompareTag("Player"))
            {
                GameManager.GM.Damage();
                //Calculate distance and direction
                dist = Vector3.Distance(velocity.transform.position, transform.position);
                dir = (velocity.transform.position - transform.position).normalized;  
                explosionForce = 15 / dist;
                if (explosionForce >= 15)
                {
                    explosionForce = 15;
                }
                velocity.velocity = new Vector3(dir.x * explosionForce / 5, explosionForce, dir.z * explosionForce / 5);


                //Damage to player
                int damage = (int)Math.Round(explosionForce);
                
                health.hp = health.hp - damage;     

                

            }
            



        }
        
    }
    
    
}
