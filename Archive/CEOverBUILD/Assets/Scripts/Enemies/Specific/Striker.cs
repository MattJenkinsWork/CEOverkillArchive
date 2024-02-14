using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MapValues;

public class Striker : Enemy
{
    [Header("Striker Stats")]
    //How far away should the striker be before it stops
    public float stopDistance;

    //How much time should pass between each hit
    public float timeBetweenHits = 1;

    //How much time should pass the first time that they hit you
    public float firstHitTime = 0.1f;

    //Is the striker currently stopped?
    private bool stopped = false;

    //Animator for controlling animations
    Animator anim;

    //Internal variable for choosing if 
    bool canAntiFlock = true;

    //How far must the enemy be away from another enemy to not have to move.
    public float antiFlockDistance = 2;

    //How far is the maximum the enemy moves when they sense another enemy too close
    public float antiFlockMoveRange = 10;

    //How close must the enemy be to the randomly chosen destination to say they've got there 
    public float antiFlockDestinationTolerance = 5;

    [Header("Leap")]


    [Range(0, 100)]
    //Chance of beginning a leap while within correct range
    public float leapChance;

    //the maximum distance away that an enemy can be before leaping
    public float maxLeapDistance;

    //the minimum distance away that an enemy can be before leaping
    public float minLeapDistance;

    //The maximum height the striker can go upwards or downwards when leaping
    public float maxLeapHeight;

    //the startup time of a leap
    public float leapStartDelay;

    //the in air leap speed
    public float leapSpeed;

    //the time it takes to reset the ability to leap after a leap is done
    public float leapResetTime;

    //how close the player must be in oder to get hit by the leap
    public float leapHitRange;

    //The height that the leap reaches before descending
    public float apexHeight;

    //Where the striker is leaping to
    Vector3 leapDestination;

    //The apex of the striker's leap
    Vector3 apexOfLeap;

    //are we in the startup phase of the leap. Used to prevent coroutine spam
    bool isBeginningLeap;

    //are we leaping currently
    bool isLeaping;

    //is it possible to jump on this frame
    bool canLeap;

    //Is the leap being reset / are we reloading it
    bool isResettingLeap;

    //Have we reached the apex of the current leap
    bool reachedApex = false;

    //Is this the first hit in the encounter
    bool isFirstHit = true;

    private void Start()
    {
        //Getting the animator
        anim = transform.GetChild(2).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug to make enemies dodge
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    aiState = AIState.dodge;
        //}


        //tracks and follows the player.Will stop within a certain range of the player.
        if (Vector3.Distance(transform.position, player.transform.position) > stopDistance)
        {
            stopped = false;
            isFirstHit = true;
            //nav.enabled = true;
        }
        else
        {
            stopped = true;
            //nav.enabled = false;
        }

        if (Vector3.Distance(transform.position, player.transform.position) > maxLeapDistance || Vector3.Distance(transform.position, player.transform.position) < minLeapDistance || isResettingLeap || stunned)
        {
            canLeap = false;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < maxLeapDistance && LeapChanceHandler())
        {
            //Debug.Log("canleap");
            canLeap = true;
        }

        if (stunned)
        {
            AnimUpdate();
        }

        RunStateMachine();


    }

    bool LeapChanceHandler()
    {
        return (Random.Range(0, 100) < leapChance);
    }


    public override void ReEvaluateState()
    {
        AnimUpdate();


        dodgeBox.isSensing = true;

        //Can we see the player and is he attacking and can we dodge and are we in a dodgeable state
        if (dodgeBox.isSeeingPlayer && (aiState == AIState.chase || aiState == AIState.fire || aiState == AIState.idle || aiState == AIState.circle))//Plus more states here if required
        {

            if (gameManager.isPlayerAttacking && canDodge)
            {
                //Debug.Log("Moving to dodge state");
                //Debug.Log("I dodged bro");
                aiState = AIState.dodge;
                return;
            }


        }

        if (Vector3.Distance(transform.position, player.transform.position) < stopDistance)
        {
            aiState = AIState.extraState1;
            return;

        }

        if (canLeap)
        {
            //Debug.Log("Reached leap ");
            aiState = AIState.extraState2;
            return;
        }


        if (Vector3.Distance(player.transform.position, transform.position) < chaseTolerance && !canLeap)
        {
            aiState = AIState.chase;
            return;
        }
        //Decision on whether to chase, attack or 








        //If nothing else is triggered, we go idle
        aiState = AIState.idle;
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
            case 7:
                LeapHandler();
                break;
        }


    }



    void LeapHandler()
    {
        nav.velocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        AnimUpdate();
        //Start the startup, establish if we're able to jump or not
        if (!isLeaping && !isBeginningLeap)
        {
            leapDestination = pManager.enemyTarget;

            RaycastHit[]  wallHit;

            wallHit = Physics.SphereCastAll(transform.position, 5, (leapDestination - transform.position).normalized, Vector3.Distance(transform.position,leapDestination) * 1.1f);

            bool wallObstruction = false;

            for (int i = 0; i < wallHit.Length; i++)
            {
                if (wallHit[i].collider.gameObject.CompareTag("Hookable"))
                {
                    //Debug.Log("Obst");
                    wallObstruction = true;
                    break;
                }
            }
                

            RaycastHit hit;

            if (Physics.Raycast(leapDestination, Vector3.down, out hit, 4) && 
                !hit.collider.gameObject.CompareTag("Hookable") && !hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.CompareTag("Enemy") && 
                hit.collider.gameObject.layer != 31 && 
                Mathf.Abs(transform.position.y - leapDestination.y) < maxLeapHeight &&
                !wallObstruction)
                
            {
                StartCoroutine(LeapStartup());
                return;
            }
            else
            {
                //Debug.Log("Failed a jump");
                aiState = AIState.idle;
                return;
            }

            
            
        }
        else if (!isLeaping)
        {
            return;
        }


        if (!reachedApex)
        {
            transform.position = Vector3.MoveTowards(transform.position, apexOfLeap, leapSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, apexOfLeap) < 1)
            {
                reachedApex = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, leapDestination, leapSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, leapDestination) < 1)
            {
                SFX_Hooker.instance.OnStrikerLand(transform.position);
                isLeaping = false;
                nav.enabled = true;
                nav.velocity = Vector3.zero;
                aiState = AIState.idle;
                AnimUpdate();
                StartCoroutine(LeapReset());

                if (Vector3.Distance(transform.position, player.transform.position) < leapHitRange)
                {
                    player.GetComponent<PlayerManager>().PlayerTakeDamage(damage);
                }

            }
        }


    }

    IEnumerator LeapStartup()
    {
        SFX_Hooker.instance.OnStrikerBuildup(transform.position);
        isBeginningLeap = true;
        yield return new WaitForSeconds(leapStartDelay);
        reachedApex = false;

        LookAtObject(leapDestination);

        Ray ray = new Ray();

        ray.origin = transform.position;
        ray.direction = transform.forward;

        apexOfLeap = ray.GetPoint(Vector3.Distance(transform.position,leapDestination) / 2);
        apexOfLeap += Vector3.up * apexHeight;

        isLeaping = true;
        isBeginningLeap = false;
        nav.enabled = false;
    }

    IEnumerator LeapReset()
    {
        isResettingLeap = true;
        yield return new WaitForSeconds(leapResetTime);
        isResettingLeap = false;
    }


    void AttackState()
    {
        //Debug.Log("I'm attacking you!");


        //Stops the striker and begins to attack
        if (!isFiring && stopped)
        {
            StartCoroutine(AttackWait());
            nav.SetDestination(transform.position);
            nav.autoBraking = true;
        }



        if (!stopped)
            ReEvaluateState();
    }


    //Attacks if the striker is still stopped when the coroutine ends
    IEnumerator AttackWait()
    {
        isFiring = true;

        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Striker_Chargeup");
        if (isFirstHit)
        {
            yield return new WaitForSeconds(firstHitTime);
            isFirstHit = false;
        }
        else
        {
            yield return new WaitForSeconds(timeBetweenHits);
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Striker_Attacking");

        if (stopped == true)
        {
            SFX_Hooker.instance.OnStrikerAttack(transform.position);
            pManager.PlayerTakeDamage(damage);
            gameManager.gameObject.GetComponent<UiManager>().HitIndicatorData(transform.position);
        }


        nav.autoBraking = false;
        isFiring = false;
    }

    //Updates the animations on this enemy
    void AnimUpdate()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", false);

        if (stopped) //&& aiState == AIState.extraState1)
        {
            anim.SetBool("isShortAttack", true);
            return;
        }
        else
        {
            anim.SetBool("isShortAttack", false);
        }


        if (aiState == AIState.chase)
        {
            //Debug.Log(nav.velocity.magnitude);
            //anim.speed = MapValuesExtension.Map(nav.velocity.magnitude, 0, 17.8f, 0.4f, 1.1f);
            anim.SetBool("isRunning", true);
            anim.SetBool("isShortAttack", false);
        }



        if (aiState == AIState.idle && nav.velocity.magnitude < Vector3.one.magnitude)
        {
            anim.speed = 1;
            anim.SetBool("isIdle", true);
        }
        else if (aiState == AIState.idle)
        {
            //anim.speed = MapValuesExtension.Map(nav.velocity.magnitude, 0, 100, 0.2f, 1);
            anim.SetBool("isRunning", true);
        }

        if (stunned)
        {
            anim.SetBool("isIdle", true);
        }


        //Debug.Log(isLeaping);
        if (isLeaping)
        {
            //Debug.Log("I should be jumping");
            anim.SetBool("isRunning", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isLeaping", true);
        }

        if (!isLeaping)
        {
            anim.SetBool("isLeaping", false);
        }

        //print(nav.velocity.magnitude);

        

    }

    public override void IdleBehaviour()
    {
        //Handles anti flocking by making the enemy run to a random position if they spot another enemy too close
        if (canAntiFlock)
        {
            nav.autoBraking = true;

            Collider[] cols = Physics.OverlapSphere(transform.position, antiFlockDistance);

            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.CompareTag("Enemy") && cols[i].gameObject != gameObject)
                {
                    nav.SetDestination(new Vector3(transform.position.x + Random.Range(-antiFlockMoveRange, antiFlockMoveRange), transform.position.y, transform.position.z + Random.Range(-antiFlockMoveRange, antiFlockMoveRange)));
                    canAntiFlock = false;
                    return;
                }
            }
        }


        if (Vector3.Distance(transform.position, nav.destination) < antiFlockDestinationTolerance)
        {
            nav.autoBraking = false;
            nav.velocity = Vector3.zero;
            canAntiFlock = true;
        }





        base.IdleBehaviour();

    }

    public override void EnemyDead(DeathType deathInput)
    {
        SFX_Hooker.instance.OnStrikerDamaged(transform.position);
        base.EnemyDead(deathInput);
    }

}

