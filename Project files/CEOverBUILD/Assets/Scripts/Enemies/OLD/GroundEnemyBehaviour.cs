using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemyBehaviour : MonoBehaviour {

    public NavMeshAgent agent;

    Transform player;

    float speed;


    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        speed = Random.Range(agent.speed - 2, agent.speed + 2);
        agent.speed = speed;
    }
	
	// Update is called once per frame
	void Update () {
        agent.SetDestination(player.position);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.Mouse0))
            Destroy(this.gameObject);
    }



}
