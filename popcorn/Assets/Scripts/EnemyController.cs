using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public PlayerController player;
    public NavMeshAgent agent;

    [Header("Enemy Stats")]
    public int health = 3;
    public int maxHealth;
    public int damageGiven = 1;
    public int damageRecieved = 1;
    public float pushBackForce = 10000;
        
        
        // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("popcorn").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.transform.position;

        if (health <= 0)
            Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "bullet")
        {
            health -= damageRecieved;
            Destroy(collision.gameObject);
          

        }
                
            
    }

}
