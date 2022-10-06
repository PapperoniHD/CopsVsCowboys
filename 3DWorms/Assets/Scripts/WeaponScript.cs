using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
public class WeaponScript : GameManager
{
    //GameObjects
    [Header("GameObjects")]
    public Rigidbody grenadePrefab;
    public GameObject grenadeHand;
    public GameObject revolverHand;
    public CinemachineVirtualCamera fpsCamera;
    public CinemachineFreeLook tpsCamera;
    public CinemachineBrain cm;
    public GameObject Player;

    public float bulletForce = 200;
    public Transform bulletSpawn;
    public Rigidbody bullet;
    public Image grenade;
    public Image pistol;
    public Image crosshair;
    public Slider charge;
    
    [Header("Speed And Velocity Variables")]
    // horizontal rotation speed
    public float horizontalSpeed = 1f;
    // vertical rotation speed
    public float verticalSpeed = 1f;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    
    
    //Variables
    public float grenadeVelocity;
    public float grenadeVelocityMultiplier = 3;
    public float grenadeMaxVelocity = 10;
    public int grenadeAmmo = 2;
    public bool grenadeActive;
    public bool revolverActive;
    public int ammo;
    public int revolverAmmo = 1;
    private Animator anim;
    public float pistolWait;
    public float pistolReset = 1;
    public bool isAiming;
    public float rocketVelocity = 7;

    //UI
    public TMP_Text ammoUI;


    // Start is called before the first frame update
    void Start()
    {
        CrossHairOff();;
        grenadeActive = false;
        charge.gameObject.SetActive(false);
        fpsCamera.Priority = 0;
        tpsCamera.Priority = 1;
        grenadeVelocity = 1;

        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerMovement>().playerActive && GameManager.GM.State == GameState.Movement)
        {     
            InputManager();
            ChargeHandler();

            ammoUI.SetText("Ammo: " + ammo.ToString("F0"));

            if (revolverActive)
            {
                ammo = revolverAmmo;
            }
            else if (grenadeActive)
            {
                ammo = grenadeAmmo;
            }
        }
        else
        {
            fpsCamera.Priority = 0;
            tpsCamera.Priority = 0;
        }


        if (GameManager.GM.State != GameState.Movement)
        {
            CrossHairOff();
        }


        if (revolverActive && revolverAmmo <= 0)
        {
            revolverActive = false;
        }
        if (grenadeActive && grenadeAmmo <= 0)
        {
            grenadeActive = false;
        }

        if (!revolverActive && !grenadeActive)
        {
            isAiming = false;
        }
        

    }



    void InputManager()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && !grenadeActive && grenadeAmmo > 0)
        {
            grenadeActive = true;
            revolverActive = false;
            ammo = grenadeAmmo;
            
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1) && grenadeActive)
        {
            grenadeActive = false;
            GetComponentInParent<PlayerMovement>().enabled = true;
        }
        
        if (grenadeAmmo <= 0)
        {
            GetComponentInParent<PlayerMovement>().enabled = true;
        }

        if (revolverAmmo <= 0)
        {
            GetComponentInParent<PlayerMovement>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && !revolverActive && revolverAmmo > 0)
        {
            revolverActive = true;
            grenadeActive = false;
            ammo = revolverAmmo;
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && revolverActive)
        {
            revolverActive = false;
        }
        
        if (grenadeActive && grenadeAmmo > 0)
        {
            grenade.enabled = true;
            grenadeHand.SetActive(true);
            revolverActive = false;
            GrenadeManager();
        }
        else
        {
            grenade.enabled = false;
            grenadeHand.SetActive(false);
            fpsCamera.Priority = 0;
            tpsCamera.Priority = 1;
            CrossHairOff();
        }

        if (revolverActive && revolverAmmo > 0)
        {
            charge.gameObject.SetActive(false);
            pistol.enabled = true;
            revolverHand.SetActive(true);
            RevolverManager();
        }
        else
        {
            pistol.enabled = false;
            revolverHand.SetActive(false);
            CrossHairOff();
        }
        
        
        
        
    }

    void GrenadeManager()
    {
        grenadeHand.SetActive(true);
        if (Input.GetKey(KeyCode.Mouse1))
        {
            CrosshairOn();
            anim.SetBool("walking", false);
            cm.m_DefaultBlend.m_Time = 0.2f;
            fpsCamera.Priority = 1;
            tpsCamera.Priority = 0;
            AimMovement();
            isAiming = true;
       
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                
                charge.gameObject.SetActive(false);
                GameManager.GM.Observing();
                Rigidbody clone;
                clone = Instantiate(grenadePrefab, grenadeHand.transform.position, grenadeHand.transform.rotation);
                
                clone.velocity = transform.TransformDirection(Vector3.forward * grenadeVelocity);
                
                grenadeAmmo--;
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                charge.gameObject.SetActive(true);
                grenadeVelocity += Time.deltaTime * grenadeVelocityMultiplier;

                if (grenadeVelocity >= grenadeMaxVelocity)
                {
                    grenadeVelocity = grenadeMaxVelocity;
                }
            }
            else
            {
                grenadeVelocity = 1;
                charge.gameObject.SetActive(false);
            }
            
        }
        else
        {
            cm.m_DefaultBlend.m_Time = 2f;
            fpsCamera.Priority = 0;
            tpsCamera.Priority = 1;
            CrossHairOff();
            isAiming = false;
        }
    }

    void RevolverManager()
    {
        revolverHand.SetActive(true);
        if (Input.GetKey(KeyCode.Mouse1))
        {
            CrosshairOn();
            anim.SetBool("walking", false);
            cm.m_DefaultBlend.m_Time = 0.2f;
            fpsCamera.Priority = 1;
            tpsCamera.Priority = 0;
            AimMovement();
            isAiming = true;
      
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                
                charge.gameObject.SetActive(false);
                GameManager.GM.Observing();
                Rigidbody clone;
                clone = Instantiate(bullet, revolverHand.transform.position, revolverHand.transform.rotation);
                
                clone.velocity = transform.TransformDirection(Vector3.forward * rocketVelocity);
                
                revolverAmmo--;
            }
            else
            {
                grenadeVelocity = 1;
                charge.gameObject.SetActive(false);
            }
            
        }
        else
        {
            cm.m_DefaultBlend.m_Time = 2f;
            fpsCamera.Priority = 0;
            tpsCamera.Priority = 1;
            CrossHairOff();
            isAiming = false;
        }
    }
    

    private void AimMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;
 
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30, 50);
 
        Player.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
    }


    void ChargeHandler()
    {
        charge.value = grenadeVelocity;
    }


    void CrosshairOn()
    {
        crosshair.enabled = true;
    }

    void CrossHairOff()
    {
        crosshair.enabled = false;
    }
    
    void PistolKnockback()
    {
        GetComponent<PlayerMovement>().velocity = -transform.forward * 5 * Time.deltaTime;
    }
    
}
