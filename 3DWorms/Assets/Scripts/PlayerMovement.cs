using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public Transform cam;
    public float speed = 1;
    [SerializeField] 
    private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
   //Gravity Variables
    [SerializeField]
    float gravity = 9.81f;
    //float groundedGravity = -0.5f;

    public Vector3 velocity;
    bool isGrounded;
    bool touchLava;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public LayerMask lavaMask;
    public float jumpHeight = 1;

    public bool grounded;
    public bool isJumping;
    public bool playerActive;


    public ParticleSystem lavaOuch;

    //Sound Effects
    public AudioSource jumpSound;
    public AudioSource walkSound;

    //Animation
    Animator anim;

    //UI


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        
        if (!Input.GetKey(KeyCode.Space) && GetComponent<WeaponScript>().isAiming == false)
        { Movement(); }

        if (GameManager.GM.State == GameState.Damage )
        {
            groundDistance = 0.5f;
        }
        else
        {
            groundDistance = 0.2f;
        }
        
        Gravity();
        
        VelocityDecrease();
    }

    void Movement()
    {
        //Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        
        
        
        //Movement
        if (direction.magnitude >= 0.1f && playerActive && GameManager.GM.State == GameState.Movement)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            
            anim.SetBool("walking", true);

            if (!walkSound.isPlaying && isGrounded)
            { 
               walkSound.Play();                                
            }

        }
        else
        {
            walkSound.Stop();
            
            anim.SetBool("walking", false);
        }
        
    }

    void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        touchLava = Physics.CheckSphere(groundCheck.position, groundDistance, lavaMask);
        controller.Move(velocity * Time.deltaTime);

        //Jump and Charging

        if (jumpHeight >= 10f)
        {
            jumpHeight = 10f;
        }

        if (touchLava && !isGrounded)
        {
            lavaOuch.Play();
            velocity = new Vector3(Random.Range(-2f,2f), 10, Random.Range(-2f,2f));
            GetComponent<PlayerHealth>().hp -= Random.Range(2,6);
        }
        
        if (isGrounded)
        {
            grounded = true;

            if (velocity.y <= -5)
            {
                velocity.y = -5f;
            }

            if (playerActive)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    jumpHeight += Time.deltaTime * 5;
                    anim.SetBool("walking", false);
                    walkSound.Stop();
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    if (jumpHeight >= 2)
                    {
                        if (!jumpSound.isPlaying)
                        {
                            jumpSound.Play();
                        }
                        walkSound.Stop();
                        speed = jumpHeight / 2;
                        print("jump");
                        velocity.y = jumpHeight;
                    }
                    jumpHeight = 1;
                }
            }
            else
            {
                return;
            }
            
        }
        else
        {
            grounded = false;
            velocity.y -= gravity * Time.deltaTime;
        }

        speed -= Time.deltaTime * 2;

        if (speed <= 1)
        {
            speed = 1;
        }
    }


    public void VelocityDecrease()
    {
        
        if (GameManager.GM.State == GameState.Damage || GameManager.GM.State == GameState.Movement || GameManager.GM.State == GameState.Killing)
        {

            InvokeRepeating("GroundedVelocity",0.5f,0.5f);
            
            if (velocity.x == 0 && velocity.y == 0) 
            {
                velocity.x = 0;
                velocity.z = 0;
            }
            else if (velocity.x > 0 && velocity.z > 0)
            {
                velocity.x -= Time.deltaTime; //* GetComponent<GrenadeScript>().explosionForce;
                velocity.z -= Time.deltaTime; //* GetComponent<GrenadeScript>().explosionForce;
            }
            else if (velocity.x < 0 && velocity.z < 0)
            {
                velocity.x += Time.deltaTime; //* GetComponent<GrenadeScript>().explosionForce;
                velocity.z += Time.deltaTime; //* GetComponent<GrenadeScript>().explosionForce;

            }
        }
        else
        {
            CancelInvoke("GroundedVelocity");
        }
    }

    public void GroundedVelocity()
    {
        if (isGrounded)
        {
            velocity.x = 0;
            velocity.z = 0;
            
        }
    }
}
