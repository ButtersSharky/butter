using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody popcorn;
    Camera playerCam;

    Vector2 camRotation;

    public Transform weaponSlot; 

    [Header("Player Stats")]
    public int maxHealth = 5;
    public int health = 5;
    public int healthRestore = 1;

    [Header("Weapon Stats")]
    public bool canFire = true;

    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sprintMultiplier = 2.5f;
    public bool sprintMode = false;
    public float jumpHeight = 5.0f;
    public float groundDetectDistance = 1f;

    [Header("User Settings")]
    public bool sprintToggleOption;
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2.0f;
    public float Ysensitivity = 2.0f;
    public float camRotationLimit = 90f;

    // Start is called before the first frame update
    void Start()
    {
        popcorn = GetComponent<Rigidbody>();
        playerCam = transform.GetChild(0).GetComponent<Camera>();

        camRotation = Vector2.zero;
        Cursor.visible = false;
         Cursor.lockState = CursorLockMode.Locked;

    } 
    

    // Update is called once per frame
    void Update()
    {
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        playerCam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);


        Vector3 temp = popcorn.velocity;

        float verticalMove = Input.GetAxisRaw("Vertical");
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
    private void OnCollisionEnter(Collision collision)
    {
        if ((health < maxHealth) && collision.gameObject.tag == "health pickup")
        {
            health += healthRestore;

            if (health > maxHealth)
                health = maxHealth;

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "weapon")
            collision.gameObject.transform.SetParent(weaponSlot);
    }



    private void cooldown(bool condition, float timeLimit)
    {
        float timer = 0;

        if (timer > timeLimit)
            timer += Time.deltaTime;

        else
            condition = true; 
    }


    IEnumerator cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true; 
    }





}