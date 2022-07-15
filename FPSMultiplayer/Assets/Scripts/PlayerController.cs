using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("References")]
    public Transform viewPoint;
    public CharacterController carCon;
    private Camera cam;
    public GameObject bulletImpact;

    [Header("Variables")]
    public float mouseSensitivity = 1f;
    public float gravityMod = 2.5f;
    public float JumpForce = 5f;
    
    public Transform GroundCheckPoint;
    private bool isGround;
    public LayerMask groundLayer;
    
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    private float verticalRotStore;
    private float activeMoveSpeed;

  //  public float timeBetweenShots = .1f;
    private float shotCounter;

    public float maxHeatValue = 10f, /*heatPerShot = 1f, */ coolRate = 4f, overheatCoolRate = 5f;
    private float heatCounter=0f;
    private bool overHeated;
    private Vector2 mouseInput;
    private Vector3 MoveDirection, movement;

    public float muzzleDisplayTime;
    private float muzzleCounter;

    public Gun[] allGuns;
    private int selectedGun= 0;

    public GameObject playerHitImpact;
    GameObject bulletImpactObject;

    public int maxHealth = 100;
    private int  currentHealth;

    public Animator playerAnimator;

    public GameObject playerModel;

    public Transform modelGunPoint;
    public Transform gunHolder;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
        //switchGun(); 
        photonView.RPC("setGun", RpcTarget.All, selectedGun);
        UIController.instance.weaponTempSlider.maxValue = maxHeatValue;
        currentHealth = maxHealth;
        
       if(photonView.IsMine)
        {
            playerModel.SetActive(false);
            UIController.instance.playerHealth.maxValue = maxHealth;
            UIController.instance.playerHealth.value = maxHealth;
        }
       else
        {
            gunHolder.parent = modelGunPoint;
            gunHolder.localPosition = Vector3.zero;
            gunHolder.localRotation = Quaternion.identity;
        }
    }
    private void Update()
    {
        if(photonView.IsMine)
        {
            PlayerLook();
            PlayerMove();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Cursor.lockState == CursorLockMode.None)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            if (allGuns[selectedGun].muzzleFlash.activeInHierarchy)
            {
                muzzleCounter -= Time.deltaTime;
                if (muzzleCounter <= 0)
                {
                    allGuns[selectedGun].muzzleFlash.SetActive(false);
                }

            }

            if (!overHeated)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    shoot();
                }


                if (Input.GetMouseButton(0) && allGuns[selectedGun].isAutomatic)

                {
                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        shoot();
                    }
                }
                heatCounter -= coolRate * Time.deltaTime;
            }
            else
            {
                heatCounter -= overheatCoolRate * Time.deltaTime;
                if (heatCounter <= 0)
                {
                    overHeated = false;
                    UIController.instance.overheatedText.gameObject.SetActive(false);
                    heatCounter = 0;
                }
            }
            if (heatCounter < 0)
            {
                heatCounter = 0;
            }
            UIController.instance.weaponTempSlider.value = heatCounter;


            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                selectedGun++;
                if (selectedGun >= allGuns.Length)
                {
                    selectedGun = 0;
                }
                photonView.RPC("setGun", RpcTarget.All, selectedGun);
                //switchGun();
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                selectedGun--;
                if (selectedGun < 0)
                {
                    selectedGun = allGuns.Length - 1;
                }
                photonView.RPC("setGun", RpcTarget.All, selectedGun);
                //switchGun();
            }

            for (int i = 0; i < allGuns.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    selectedGun = i;
                    photonView.RPC("setGun", RpcTarget.All, selectedGun);
                    // switchGun();
                }
            }
            playerAnimator.SetBool("grounded", isGround);
            playerAnimator.SetFloat("speed",MoveDirection.magnitude);
        }
        
    }

   private void shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
           if(hit.collider.gameObject.tag =="Player")
            {

                Debug.Log("Hit "+ hit.collider.gameObject.GetPhotonView().Owner.NickName);
                PhotonNetwork.Instantiate(playerHitImpact.name, hit.point,Quaternion.identity);

                hit.collider.gameObject.GetPhotonView().RPC("dealDamage",RpcTarget.All,photonView.Owner.NickName, allGuns[selectedGun].shotDamage, PhotonNetwork.LocalPlayer.ActorNumber);

            }
           else
            {
                bulletImpactObject = Instantiate(bulletImpact, hit.point + (hit.normal * .002f), Quaternion.LookRotation(hit.normal, Vector3.up));

                Destroy(bulletImpactObject, 10f);
            }

             
        }

        shotCounter = allGuns[selectedGun].timeBetweenShots;
        heatCounter += allGuns[selectedGun].hearPerShot;
        if(heatCounter>=maxHeatValue)
        {
            heatCounter = maxHeatValue;
            overHeated = true;
            UIController.instance.overheatedText.gameObject.SetActive(true);
        }

        allGuns[selectedGun].muzzleFlash.SetActive(true);
        muzzleCounter = muzzleDisplayTime;
    }

    [PunRPC]
    public void dealDamage(string damager, int damageAmount,int actor)
    {
        takeDamage(damager, damageAmount,actor);
    }

    public void takeDamage(string damager, int damageAmount,int actor)
    {  
        if(photonView.IsMine)
        {
            currentHealth -= damageAmount;
            UIController.instance.playerHealth.value = currentHealth;
            if(currentHealth<=0)
            {
                currentHealth = 0;
                MatchManager.instance.UpdateStatsSend(actor,0,1);
                PlayerSpawner.instance.die(damager);
                
            }
            
        }
       
    }
    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            cam.transform.position = viewPoint.position;

            cam.transform.rotation = viewPoint.rotation;
        }
    }
    void PlayerLook()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);


        verticalRotStore += mouseInput.y;
        verticalRotStore = Mathf.Clamp(verticalRotStore, -60, 60);


        viewPoint.rotation = Quaternion.Euler(-verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);


    }
    void PlayerMove()
    {
        MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = sprintSpeed;
        }
        else
        {
            activeMoveSpeed = walkSpeed;
        }

        float yVel = movement.y;

        movement = ((transform.forward * MoveDirection.z) + (transform.right * MoveDirection.x)).normalized * activeMoveSpeed;

        movement.y = yVel;

        if(carCon.isGrounded)
        {
            movement.y = -0.2f ;
            
        }

        isGround = Physics.Raycast(GroundCheckPoint.position,Vector3.down,0.25f,groundLayer);

        if (Input.GetButtonDown("Jump") && isGround)
        {
            movement.y = JumpForce;
           
        }

        movement.y += Physics.gravity.y *  gravityMod*Time.deltaTime;

        carCon.Move(movement * Time.deltaTime);
    }
    
   void switchGun()
    {
        for(int i=0;i<allGuns.Length;i++)
        {
            if(i==selectedGun)
            {
                allGuns[i].gameObject.SetActive(true);

            }
            else
            {
                allGuns[i].gameObject.SetActive(false);
            }
        }
        allGuns[selectedGun].muzzleFlash.SetActive(false);
    }

    [PunRPC]
    public void setGun(int gunToSwitchTo)
    {
        if(gunToSwitchTo<allGuns.Length)
        {
            selectedGun = gunToSwitchTo;
            switchGun();
        }
    }
}

