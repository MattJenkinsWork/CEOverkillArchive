using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplodingStriker : Enemy
{
    //How far away should the striker be before it stops
    public float stopDistance;
    public float timeBetweenHits = 1;
    private bool stopped = false;
    public float distance;
    public GameObject explosionBox;

    private void Start()
    {
        explosionBox = transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        RunStateMachine();

        //tracks and follows the player. Will stop within a certain range of the player.
        if (Vector3.Distance(transform.position, player.transform.position) > stopDistance)
        {
            //Makes the striker avoid flocking and keep following the player
            stopped = false;
        }
        else
        {
            //The player is in range and thus the AIstate is set to the extraStateHandler
            aiState = AIState.extraState1;
            stopped = true;
        }

    }


    public override void DoDeathEffects()
    {

    }

    //Extra state 6 is used here in order to create an attack state
    public override void ExtraStateHandler(AIState aiState)
    {
        //Debug.Log("It's ya boi extra state handler");

        switch ((int)aiState)
        {
            case 6:
                AttackState();
                break;
        }


    }

    void AttackState()
    {
        //Debug.Log("I'm attacking you!");

        if (!isFiring && stopped)
            StartCoroutine(AttackWait());

        if (!stopped)
            aiState = AIState.idle;
    }


    //Colour hack is bad. FIX ME
    IEnumerator AttackWait()
    {
        isFiring = true;

        //transform.GetChild(2).GetComponent<Renderer>().material.color = Color.red;
        Debug.Log("fuck");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Striker_Chargeup");
        yield return new WaitForSeconds(timeBetweenHits);// 2
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Striker_Attacking");
        explosionBox.SetActive(true);
        explosionBox.transform.SetParent(null);
        Destroy(gameObject);
        //transform.GetChild(2).GetComponent<Renderer>().material.color = Color.white;


        if (stopped == true)
        {
            pManager.PlayerTakeDamage(damage);
            gameManager.gameObject.GetComponent<UiManager>().HitIndicatorData(transform.position);
        }



        isFiring = false;
    }

}
