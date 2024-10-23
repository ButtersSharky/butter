using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody popcorn;
    Camera playerCam;

    Transform cameraHolder;

    Vector2 camRotation;

    public Transform weaponSlot;

    public GameManager gm;

    [Header("Player Stats")]
    public int maxHealth = 5;
    public int health = 5;
    public int healthRestore = 1;

    [Header("Weapon Stats")]
    public AudioSource weaponSpeaker;
    public GameObject shot;
    public float shotVel = 0;
    public int weaponID = -1;
    public int fireMode = 0;
    public float fireRate = 0;
    public float currentClip = 0;
    public float clipsize = 0;
    public float maxAmmo = 0;
    public float currentAmmo = 0;
    public float reloadAmt = 0;
    public float bulletLifespan = 0;
    public bool canFire = true;

    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sprintMultiplier = 2.5f;
    public bool sprintMode = false;
    public float jumpHeight = 5.0f;
    public float groundDetectDistance = 1f;

    [Header("User Settings")]
    public bool sprintToggleOption = false;
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2.0f;
    public float Ysensitivity = 2.0f;
    public float camRotationLimit = 90f;

    // Start is called before the first frame update
    void Start()
    {
        popcorn = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        cameraHolder = transform.GetChild(0);
        

        camRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.isPaused)
        {
            playerCam.transform.position = cameraHolder.position;

            camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

            playerCam.transform.rotation = Quaternion.Euler(-camRotation.y, camRotation.x, 0);
            transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

            if (Input.GetMouseButton(0) && canFire && currentClip > 0 && weaponID >= 0)
            {
                GameObject s = Instantiate(shot, weaponSlot.position, weaponSlot.rotation);
                s.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * shotVel);
                Destroy(s, bulletLifespan);

                canFire = false;
                currentClip--;
                StartCoroutine("cooldownFire");
            }


            if (Input.GetKeyDown(KeyCode.R))
                reloadClip();

            Vector3 temp = popcorn.velocity;

        }   float verticalMove = Input.GetAxisRaw("Vertical");
            float horizontalMove = Input.GetAxisRaw("Horizontal");

        if (!sprintToggleOption)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                sprintMode = true;

            if (Input.GetKey(KeyCode.LeftShift))
                sprintMode = false;
        }

        if (sprintToggleOption)
        {
            if (Input.GetKey(KeyCode.LeftShift) && verticalMove > 0)
                sprintMode = true;

            if (verticalMove <= 0)
                sprintMode = false;

        }

            if (!sprintMode)
                temp.x = verticalMove * speed;

            if (sprintMode)
                temp.x = verticalMove * speed * sprintMultiplier;

            temp.z = Input.GetAxisRaw("Horizontal") * speed;

         //if (Input.GetKey(KeyCode.LeftShift))

        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
            temp.y = jumpHeight;

        popcorn.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "weapon")
        {

            other.gameObject.transform.SetPositionAndRotation(weaponSlot.position, weaponSlot.rotation);


            other.gameObject.transform.SetParent(weaponSlot);


            switch (other.gameObject.name)
            {
                case "weapon1":

                    weaponID = 0;
                    shotVel = 10000;
                    fireMode = 0;
                    currentClip = 20;
                    clipsize = 20;
                    maxAmmo = 400;
                    maxAmmo = 400;
                    currentAmmo = 200;
                    reloadAmt = 20;
                    bulletLifespan = 1;
                    break;

                default:

                    break;
            }

        }

    }

        private void OnCollisionEnter(Collision collision)
        {
            if ((health < maxHealth) && collision.gameObject.tag == "health pickup")
            {
                health += healthRestore;

                if (health > maxHealth)
                    health = maxHealth;

                Destroy(collision.gameObject);
            }

            if ((currentAmmo < maxAmmo) && collision.gameObject.tag == "ammoPickup")
            {
              currentAmmo += reloadAmt;
             
              if (currentAmmo > maxAmmo)
                  currentAmmo = maxAmmo;

              Destroy(collision.gameObject);

            }

        }

        public void reloadClip()
        {
            if (currentClip >= clipsize)
                return;

            else
            {
                float reloadCount = clipsize - currentClip;

                if (currentAmmo < reloadCount)
                {
                    currentClip += currentAmmo;
                    currentAmmo = 0;
                    return;
                }

                else
                {
                   currentClip += reloadCount;
                   currentAmmo -= reloadCount;
                   return;
                }
            }
        }


        IEnumerator cooldownFire()
        {
            yield return new WaitForSeconds(fireRate);
            canFire = true;
        }
}
