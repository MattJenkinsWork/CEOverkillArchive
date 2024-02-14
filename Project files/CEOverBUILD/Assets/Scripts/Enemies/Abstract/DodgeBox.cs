using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Maintains a list of all colliders within the trigger box. If the player is in it, isSeeingPlayer is true
public class DodgeBox : MonoBehaviour {

    //Are we sensing for the player
    public bool isSensing = false;

    //The enemy we are attatched to
    GameObject attachedEnemy;
    
    //List of all gameobjects in the trigger
    List<GameObject> inTriggerList = new List<GameObject>();

    //Player reference to compare gameobjects against
    GameObject player;

    //Are we seeing the player
    public bool isSeeingPlayer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            inTriggerList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {

       for (int i = 0; i < inTriggerList.Count; i++)
       {
           if (other.gameObject == player)
           {
               inTriggerList.Remove(other.gameObject);
                isSeeingPlayer = false;
           }
       }
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(inTriggerList.Count.ToString());

        if (isSensing == true)
        {
            for (int i = 0; i < inTriggerList.Count; i++)
            {
                //Debug.Log(isSeeingPlayer + " " + inTriggerList[i].name);

                if (player == inTriggerList[i])
                {
                    isSeeingPlayer = true;
                    break;
                }
                else
                {
                    isSeeingPlayer = false;
                }
            }
        }

        
    }
}
