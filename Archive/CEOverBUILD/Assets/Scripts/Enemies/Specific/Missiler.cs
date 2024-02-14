using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiler : Enemy {

    

    //How far away should the missiler be before it stops
    [Header("Missiler")]
    public float stopDistance;

    //How long should the missiler wait between burst shots
    public float waitBetweenBursts;

    //How long it takes to fire
    public float shotBuildUpTime;

    //internal bool for forcing the missiler to run away
    bool forceRun;

    //How long after shooting is the missiler not allowed to fire for
    public float runRefresh = 5;

    LineRenderer line;

    Animator anim;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        anim = transform.GetChild(2).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

        RunStateMachine();

        //Manipulates the canFire value in order to make the state machine fire at the player
        if (Vector3.Distance(player.transform.position,transform.position) > stopDistance || forceRun)
        {
            canFire = false;
        }
        else
        {
            canFire = true;
        }

        AnimUpdate();
    }

    //An ovverride for firebehaviour without lookatplayer so that bursts stay consistent
    public override void FireBehaviour()
    {
        dodgeBox.isSensing = true;

        if (!isFiring)
            StartCoroutine(FireTimer());

        if (autoSwapAIState)
            ReEvaluateState();
    }


    //An ovveride for firetimer that does burst shots instead
    public override IEnumerator FireTimer()
    {
        isFiring = true;

        yield return new WaitForSeconds(fireRate);

        if (canFire)
        {
            line.enabled = true;
            Vector3[] points = new Vector3[2];
            points[0] = transform.position;
            points[1] = player.GetComponent<PlayerManager>().enemyTarget;
            line.SetPositions(points);

            yield return new WaitForSeconds(Random.Range(0,shotBuildUpTime));
            Fire();
            yield return new WaitForSeconds(waitBetweenBursts);
            Fire();
            yield return new WaitForSeconds(waitBetweenBursts);
            Fire();

            line.enabled = false;
            StartCoroutine("FireTimer");
            LookAtObject(pManager.enemyTarget);
            StartCoroutine(RunReset());

        }
        else
        {
            isFiring = false;
            line.enabled = false;
            StartCoroutine(RunReset());
        }
    }

    void Fire()
    {

        GameObject currentProj = Fire(projectilePrefab, new Vector3(0, 0, 0), this.gameObject);

        Projectile projAbstract = currentProj.GetComponent<Projectile>();

        projAbstract.damage = damage;
        projAbstract.lifetime = projectileLifetime;

        projRigid = currentProj.GetComponent<Rigidbody>();
        projRigid.AddForce((transform.forward + targetOffset) * projectileSpeed);

        
    }



    IEnumerator RunReset()
    {
        forceRun = true;
        runAway = true;
        yield return new WaitForSeconds(runRefresh);
        forceRun = false;
        runAway = false;
    }

    void AnimUpdate()
    {
        anim.SetBool("IDLE", false);
        anim.SetBool("SHOOTING", false);
        anim.SetBool("MOVING", false);
        anim.SetBool("DEAD", false);

        if(aiState == AIState.idle)
        {
            anim.SetBool("IDLE", true);
        }

        if(aiState == AIState.fire)
        {
            anim.SetBool("SHOOTING", true);
        }

        if(aiState == AIState.chase && pathOutput == PathOutput.found && !endOfPath)
        {
            anim.SetBool("MOVING", true);
        }
        else if (aiState == AIState.chase)
        {
            anim.SetBool("IDLE", true);
        }
    }




}
