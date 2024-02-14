using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slug : Enemy
{
    [Header("Slug Stats")]
    public static List<GameObject> slugWalls = new List<GameObject>();
    public GameObject wall;
    public Transform slugTrail;
    public Rigidbody rb;
    public float wallMinimumHeight;
    public float wallMinimumWidth;
    public float wallTolerance;
    public float cheatyWallTolerance;
    public float wallVisionRange = 5;
    public float maxRoamHeight = 100;
    bool collided = false;
    bool isSlugging = false;
    bool slimeTrail = false;
    bool trailSlime = true;
    bool up = true;
    bool canReRoam = true;
    int state = 0;
    int slimeNum;

    GameObject lookUp;

    void Start()
    {
        RunStateMachine();
        lookUp = new GameObject();
    }

    void Update()
    {

        switch (state)
        {
            case 0:
                FindWall();
                break;
            case 1:
                GroundMove();
                break;
            case 2:
                CheatyMove();
                break;
            case 3:
                WallMove();
                break;
        }
    }

    public void FindWall()
    {
        if (nav == null)
        {
            nav = gameObject.AddComponent<NavMeshAgent>();
        }


        Collider[] cols = Physics.OverlapSphere(transform.position, wallVisionRange);

        for (int i = 0; i < cols.Length; i++)
        {

            if (wallMinimumHeight < cols[i].bounds.size.y && wallMinimumWidth > cols[i].bounds.size.z && cols[i].name != "Pipe")
            {
                //Debug.Log("[Todd Howard Voice] you can climb it");
                wall = cols[i].gameObject;
                if (slugWalls.Contains(wall))
                {
                    canReRoam = true;
                }
                else
                {
                    slugWalls.Add(wall);
                    state = 1;
                    break;
                }
            }
            else
            {
                if (canReRoam)
                {
                    canReRoam = false;
                    int x = Random.Range(5, 10);
                    int z = Random.Range(5, 10);

                    GroundTrack(new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z));
                    Invoke("ReRoamReset", 10);
                }

            }
        }

        if (Vector3.Distance(transform.position, nav.destination) < 3)
        {
            ReRoamReset();
        }
    }

    void ReRoamReset()
    {
        canReRoam = true;
    }

    void NavDestroy()
    {
        Destroy(nav);
    }

    public void GroundMove()
    {

        Debug.Log("Walking towards");
        GroundTrack(wall.transform.position);
        LookAtObject(wall.transform);

        if (Vector3.Distance(transform.position, wall.transform.position) < wallTolerance)
        {
            state = 2;
        }

    }

    void CheatyMove()
    {

        Debug.Log("Cheaty");

        nav.enabled = false;

        Vector3 store = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, wall.transform.position, moveSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, store.y, transform.position.z);

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(wall.transform.position.x, 0, wall.transform.position.z)) < cheatyWallTolerance)
        {
            isSlugging = true;
            lookUp.transform.position = new Vector3(wall.transform.position.x,wall.transform.position.y + maxRoamHeight, wall.transform.position.z);
            state = 3;
        }
    }


    void WallMove()
    {
        Invoke("NavDestroy", 1.5f);

        //Debug.Log("I'm goin up");
        //Debug.Log(wall.name, wall);

        if (isSlugging == true)
        {
            StartCoroutine(SlugTrail());
            isSlugging = false;
        }

        if (up)
        {
            if(wall == null)
            {
                state = 0;
            }


            transform.position = Vector3.MoveTowards(transform.position, wall.transform.position + new Vector3(0, maxRoamHeight, 0), moveSpeed * Time.deltaTime);
            //Debug.Log(wall.transform.position + new Vector3(0, maxRoamHeight, 0));
            LookAtObject(lookUp.transform);

        }
        else
        {
            if (wall == null)
            {
                state = 0;
            }

            transform.position = Vector3.MoveTowards(transform.position, wall.transform.position, moveSpeed * Time.deltaTime);
            LookAtObject(wall.transform);
        }

        if (Vector3.Distance(transform.position, wall.transform.position + new Vector3(0, maxRoamHeight, 0)) < 2)
        {
            up = false;
        }

        if (Vector3.Distance(transform.position, wall.transform.position) < 2)
        {
            up = true;
        }


    }

    List<GameObject> slime = new List<GameObject>();

    IEnumerator SlugTrail()
    {
        slimeTrail = true;
        slimeNum = 0;

        while (slimeTrail && slime.Count < 20)
        {
            slime.Add(Instantiate(slugTrail, transform.position, transform.rotation).gameObject);
            yield return new WaitForSeconds(0.2f);
            Invoke("RemoveSlime", 3);
            slimeNum++;
        }
    }

    void RemoveSlime()
    {
        Destroy(slime[0]);
        slime.Remove(slime[0]);
    }


    //Abstracts used for additional states and fancy death stuff. DO NOT REMOVE
    public override void DoDeathEffects()
    {
        foreach (GameObject slime in slime)
        {
            Destroy(slime);
        }
    }

    public override void ExtraStateHandler(AIState aiState)
    {

    }
}


// BIG OL CODE DUMP


//Debug.Log("hit " + hit.collider.gameObject.name + "at " + distanceAway + " away");

//objectCollider = hit.collider.GetComponent<Collider>();
//objectSize = objectCollider.bounds.size;

/*
List<GameObject> wallList = wall;
_detach = wallList[Random.Range(0, wallList.Count)];
// rb.useGravity = true;
Debug.Log("Found wall");
}

void Update()
{
LookAtObject(_detach.transform);

switch (state)
{
case 0:
    GroundMove(wall);
    break;
case 1:
    WallMove();
    break;
}

if (collided == true && isSlugging == false)
{
StartCoroutine(SlugTrail());
}
}

/*
 * 
 * 
 * nav.SetDestination(wall[0].transform.position);
        //  LookAtObject(_detach.transform);

        if (transform.position.x <= _detach.transform.position.x + 0.5 && transform.position.x >= _detach.transform.position.x - 0.5
            && transform.position.z <= _detach.transform.position.z + 0.5 && transform.position.z >= _detach.transform.position.z - 0.5)
        {
            state = 1;
            Debug.Log("Moving to wall");
            Debug.Log(transform.position.z + "" + _detach.transform.position.x + 0.5);
        }

void WallMove()
{
    AirTrack(_detach.transform.position);
    LookAtObject(_detach.transform);

    moveSpeed = 1;
    Debug.Log("I'm goin up");
    collided = true;
}

IEnumerator SlugTrail()
{
    isSlugging = true;
    yield return new WaitForSeconds(1);
    Instantiate(slugTrail, transform.position, transform.rotation);
    isSlugging = false;  
}
*/
