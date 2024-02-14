using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultBot : Enemy {

    [Header("Specific Stats")]

    //If the enemy is closer than runDistance, they'll run away from the player 
    public float runDistance;

    //If the enemy is further than shootDistance, they'll get closer until they can fire again
    public float shootDistance;

    [Range(0, 100)]
    public float offsetChance = 50;

    //Supposedly how long the assault bot has been running for
    public int runCounter = 0;

    //How long the assault bot will run away for before deciding to fight
    public int runCounterMax;

    //Should the bot shoot anyway. Used by the update machine
    bool shootOverride = false;

    public float maxLaserDistance = 20;

    //Can we fire the laser
    bool laserReset = true;

    //The maximum possible offsets in each direction
    public float xOffset;
    public float yOffset;
    public float zOffset;

    Animator anim;
    bool stateLock;

	// Use this for initialization
	void Start () {
        anim = transform.GetChild(2).GetComponent<Animator>();

        GetComponent<LineRenderer>().enabled = false;
        if (runDistance > shootDistance)
            Debug.LogWarning("Assault bot shootDistance should always be above runDistance, expect odd behaviour!");

	}
	
	// Update is called once per frame
	void Update () {

        //This is a huge mess. I will redo it when dealing with enhanced ai.

        
        AnimUpdate();
        //Currently, this doesn't allow for dodging or any other state types so it may have to be changed
        //AMOUNT OF RUNAWAY ACTIONS IS CURRENTLY BROKEN

        if (Vector3.Distance(transform.position,player.transform.position) < runDistance && runCounter < runCounterMax)
        {
            canFire = false;
            runAway = true;
            aiState = AIState.chase;
            runCounter++;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < runDistance && runCounter >= runCounterMax)
        {
            shootOverride = true;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) > runDistance && Vector3.Distance(transform.position, player.transform.position) < shootDistance || shootOverride)
        {
            runAway = false;
            canFire = true;
            aiState = AIState.extraState1;
            runCounter = 0;
            shootOverride = false;
            RunStateMachine();
            return;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) > shootDistance)
        {
            canFire = false;
            runAway = false;
            aiState = AIState.chase;
        }

        RunStateMachine();



    }


    public override void ExtraStateHandler(AIState aiState)
    {
        switch ((int)aiState)
        {
            case 6:
                LaserAttack();
                break;

        }
    }

    void LaserAttack()
    {
        endOfPath = true;
        LookAtPlayer();

        //check if we're reloaded
        if (laserReset)
        {
            GetComponent<LineRenderer>().enabled = true;

            int offsetRand = Random.Range(0, 100);

            //Set ray parameters for raycasts
            Ray ray = new Ray();
            ray.direction = transform.forward;
            ray.origin = transform.position;

            //Check if we should apply an offset
            if (offsetRand < offsetChance)
            {
                ray.direction = new Vector3(Random.Range(-xOffset, xOffset) + transform.forward.x, Random.Range(-yOffset, yOffset) + transform.forward.y, Random.Range(-zOffset, zOffset) + transform.forward.z);
            }

            //If we hit something with the raycast, deal it some damage
            if (Physics.Raycast(ray, maxLaserDistance))
            {
                RaycastHit hit;
                Physics.Raycast(ray, out hit, maxLaserDistance);

                if (hit.collider.tag == "Player")
                {
                    pManager.PlayerTakeDamage(1);
                    gameManager.gameObject.GetComponent<UiManager>().HitIndicatorData(transform.position);
                }


                Vector3[] linePoints = new Vector3[2];

                linePoints[0] = transform.position;
                linePoints[1] = hit.point;

                GetComponent<LineRenderer>().SetPositions(linePoints);

                //contactPoints.Add(hit.point);

                SFX_Hooker.instance.FlowerBot_Attacking(transform.position);

                //start the reload process
                StartCoroutine(LaserReload());

                
            }
        }

    }

    //Reloads the laser
    IEnumerator LaserReload()
    {
        laserReset = false;
        yield return new WaitForSeconds(fireRate);
        laserReset = true;
        GetComponent<LineRenderer>().enabled = false;
    }

    //Sets animation parameters in order to make the ai move properly
    void AnimUpdate()
    {
        if (stateLock)
        {
            return;
        }

        anim.SetBool("isIdle", false);
        anim.SetBool("isForward", false);
        anim.SetBool("isShooting", false);
        anim.SetBool("isEvadingLeft", false);

        if (aiState == AIState.idle)
        {
            anim.SetBool("isIdle", true);
        }

        if (aiState == AIState.extraState1)
        {
            anim.SetBool("isShooting", true);
            stateLock = true;
            Invoke("StateUnlock", 0.5f);
        }

        if (aiState == AIState.chase && pathOutput == PathOutput.found && !endOfPath)
        {
            anim.SetBool("isForward", true);
        }
        else if (aiState == AIState.chase)
        {
            anim.SetBool("isIdle", true);
        }

        

        if (aiState == AIState.dodge)
        {
            anim.SetBool("isEvadingLeft", true);
        }




    }

    //Unlocks the current animation state. Always called by invoke
    void StateUnlock()
    {
        stateLock = false;
    }

    //Commented out for now, but shows where the assault bot hits with every shot
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        for (int i = 0; i < contactPoints.Count; i++)
        {
            Gizmos.DrawSphere(contactPoints[i], 0.1f);
        }
    }*/


}
