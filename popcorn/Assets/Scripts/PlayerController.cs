using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody popcorn; 

    public float speed = 10.0f;
    public float jumpHeight

    // Start is called before the first frame update
    void Start()
    {
        popcorn = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = popcorn.velocity;

        temp.x = Input.GetAxisRaw("Vertical") * speed;
        temp.z = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetKeyDown(KeyCode.Space))
            temp.y = jumpHeight
           
        popcorn.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);
       
              
       
    } 

}
