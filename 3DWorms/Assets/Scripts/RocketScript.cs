using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    //GameObjects
    public ParticleSystem explosion;
    public MeshRenderer mesh;
    public GameObject particleTrail;
    AudioSource explode;

    //Variables
    public float timer = 0f;
    public bool isPlayed = false;
    public float radius = 5f;
    public float explosionForce;
    public float dist;
    public Vector3 dir;


    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= 5f)
        {
            mesh.enabled = true;
        }
        else
        {
            if (!isPlayed)
            {     
                Instantiate(explosion, transform.position, transform.rotation);
                isPlayed = true;
                          
                Explode();
            }


            mesh.enabled = false;
        }

        if (timer >= 7)
        {
            GameManager.GM.Movement();
            Destroy(this.gameObject);
        }

        explode = GameObject.Find("ExplodeSound").GetComponent<AudioSource>();
    }

    void Explode()
    {
        explode.Play();
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Destroy(particleTrail);
       
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            PlayerHealth health = nearbyObject.GetComponent<PlayerHealth>();
            PlayerMovement velocity = nearbyObject.GetComponent<PlayerMovement>();
            if (nearbyObject.CompareTag("Player"))
            {
                
                GameManager.GM.Damage();
                
                //Calculate distance and direction and set to player
                dist = Vector3.Distance(velocity.transform.position, transform.position);

                dir = (velocity.transform.position - transform.position).normalized;
               
                explosionForce = 15 / dist;
                if (explosionForce >= 15)
                {
                    explosionForce = 15;
                }
               //Damage to Player
                int damage = (int)Math.Round(explosionForce/2);
                
                health.hp = health.hp - damage;
                
                velocity.velocity = new Vector3(dir.x * explosionForce / 5, explosionForce, dir.z * explosionForce / 5);
                
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
        timer = 5;
    }
}
