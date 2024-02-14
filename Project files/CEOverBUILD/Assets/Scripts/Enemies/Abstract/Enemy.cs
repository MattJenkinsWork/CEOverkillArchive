using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Spawn effect")]
    public GameObject spawnEffect;
    public float spawnEffectDestroyTime;

    [Header("General Stats")]
    public int maxHealth;
    public int currentHealth;
    public float moveSpeed;
    public int damage;
    public bool travelVertically;
    public bool isDead;
    public float onDeathKnockbackForce = 1000;
    public float onDeathWait = 2;
    public GameObject destroyedVersion;
    public float stunTime;
    DeathType setDeath = new DeathType();

    [Header("Projectile Stats")]
    public float fireRate;
    public float projectileSpeed;
    public float projectileLifetime;
    public GameObject projectilePrefab;

    //Used for FireTimer
    public bool canFire = false;
    public bool isFiring = false;

    //The enemy will fire always, regardless of any other variables
    public bool fireRegardless;

    [Header("AI Stats")]
    //If enabled, this automatically swaps the AI between states when certain parameters are met
    public bool autoSwapAIState = true;

    [HideInInspector]
    public bool canDodge = true;
    public float dodgeForce;

    public bool isAgent = false;

    //Dodge chance as %, 0 is no dodges, 100 is always
    [Range(0, 100)]
    public float dodgeChance;

    //The amount of time the enemy is in the dodge state for
    public float dodgeTime;

    //The amount of time before the enemy is able to dodge again
    public float dodgeWait;

    //How far away the player is allowed to be before the enemy stops chasing them
    public float chaseTolerance;

    //Will the enemy look up and down while chasing the player
    public bool lookVerticallyWhileChasing;

    //Reverses the enemy's airtrack and groundtrack, making them run away
    public bool runAway = false;

    //How far the enemy gets knocked back after being hit
    public float knockbackForce;

    //How long it takes for the enemy to return to 0 velocity
    public float knockbackResetTime;

    //The distance a flying enemy will run away if runAway becomes true
    public float runAwayDistance = 20;


    //Ai state info
    public enum AIState { spawn, idle, chase, dodge, circle, fire, extraState1, extraState2, extraState3, knockback, stun };
    public AIState aiState = AIState.spawn;

    [Header("Offset Stats")]
    public float spawnOffset;
    public Vector3 targetOffset;
    public Vector3 projectileSpawnOffset;

    //Components
    [HideInInspector]
    public Rigidbody rigid;

    [HideInInspector]
    public NavMeshAgent nav;

    //Intended to be used for fancy effects once an enemy is dead
    public enum DeathType { explosion, disintegrate, slice };

    //This enemy's dodgebox
    [HideInInspector]
    public DodgeBox dodgeBox;

    //These are all hidden because they are automatically assigned
    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public PlayerManager pManager;

    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public Rigidbody projRigid;

    //The explosion effect when an enemy dies
    public GameObject explosionEffect;

    [HideInInspector]
    //The factory for this enemy, if it has one
    public GameObject factory;

    //The current status of the pathfinding algorithm
    public enum PathOutput { inaccesible, found, processing };

    [Header("AirNavMesh Stuff")]
    public PathOutput pathOutput = PathOutput.processing;

    //Lists of nodes to access for pathfinding. 
    public List<Node> openList;
    public List<Node> closedList;
    public List<Node> currentPath;
    Node startNode;
    Node currentNode;
    Node destinationNode;

    NavmeshManager navManager;

    AirNavMesh currentNavMesh;

    public static List<Node> occupiedNodes;

    private Node storedNode;

    //How far the enemy must be to a node on the path to begin moving to the next one
    public float nodeTolerance;
    
    //How many nodes this enemy will search before giving up. Higher numbers produce more lag
    public int searchRange;

    //How far the player has to be from the end of the path before pathfinding recommences
    public float endOfPathTolerance;

    //Is this enemy at the end of their path
    public bool endOfPath;

    //How far away the enemy can possible go from their start position in meters
    public float moveDistance;

    //How far we are along the current path
    public int pathIndex;

    //How fast will an ai recommence pathfinding after it fails
    public float refreshRate = 0.5f;

    //Can the ai currently perform a pathfinding operation
    public bool canDoPath = true;

    [HideInInspector]
    //is the enemy stunned
    public bool stunned;


    private void Awake()
    {
        try
        {
            GameObject effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);
            Destroy(effect, spawnEffectDestroyTime);
        }
        catch
        {

        }



        //Setting tags
        player = GameObject.FindGameObjectWithTag("Player");
        pManager = player.GetComponent<PlayerManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        //Finding Components
        rigid = GetComponent<Rigidbody>();

        nav = GetComponent<NavMeshAgent>();

        if (isAgent)
        {
            nav = GetComponent<NavMeshAgent>();
        }
        else
        {
            nav = GetComponent<NavMeshAgent>();
            Destroy(nav);
        }


        //Resetting health
        currentHealth = maxHealth;

        //Setting up the navmesh agent
        if (isAgent)
        {
            nav.speed = moveSpeed;

        }


        //Using spawnOffset to spawn the enemy in a different position
        //transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y + spawnOffset, transform.position.y), Quaternion.identity);

        //Finding dodgebox
        dodgeBox = transform.GetChild(0).GetComponent<DodgeBox>();

        if (fireRegardless)
        {
            canFire = true;
            StartCoroutine(FireTimer());
        }

        //Setting up navmesh
        navManager = gameManager.gameObject.GetComponent<NavmeshManager>();
        currentNavMesh = navManager.currentAirNavMesh;

        //Try to set our node to be the default
        if (!isAgent)
        {
            try
            {
                currentNode = navManager.currentAirNavMesh.nodes[0];
            }
            catch
            {
                Debug.Log("Non agent cannot find airnavmesh");
            }
        }
    }


    //Spawns in the enemy, provides them with a relevant actionSpot to store
    //This is defunct but if I ever add object pools it won't be
    public GameObject Spawn(Vector3 actionSpotInput, Vector3 spawnPoint)
    {
        //instantiate at spawnpoint
        return null;

    }

    //Kills this enemy. Optionally has different modes for killing the enemy for extra graphical fanciness
    //The graphical abilities of this function are currently defunct
    public virtual void EnemyDead(DeathType deathInput)
    {
        //Creating the explosion effect
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 2);


        //If we were part of a factory, remove ourselves from the factory list

        //If a destroyed version is available, we use it
        try
        {
            factory.GetComponent<Factory>().enemiesInField.Remove(gameObject);
            
        }
        catch
        {

        }

        try
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
        }
        catch
        {

        }

        //Remove ourselves from the global active enemy list
        gameManager.RemoveEnemyList(gameObject);

        Destroy(gameObject);

        //Triggers do death effects, just in case an induvidual enemies have specific effects to deploy
        DoDeathEffects();

    }

    virtual public void DoDeathEffects()
    {
        //for overrides
    }

    //Fires a projectile spawned from a prefab
    //For now the projectile is instantiated but ideally this will swap to an object pool system eventually
    public GameObject Fire(GameObject projectile, Vector3 fireDirection, GameObject firedFrom)
    {
        //Do rotation later if required
        GameObject firedProjectile;

        firedProjectile = Instantiate(projectile, transform.position + transform.forward * 2 + projectileSpawnOffset, Quaternion.identity);

        Projectile proj = firedProjectile.GetComponent<Projectile>();
        proj.speed = projectileSpeed;
        proj.damage = damage;
        proj.firedFrom = firedFrom;

        return firedProjectile;
    }

    //This is an AI mode. Should be triggered via a state machine in the main loop of an enemy
    //Now defunct due to pathfinding existing
    public void AirTrack(Vector3 targetPos)
    {
        if (!runAway)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPos, -moveSpeed * Time.deltaTime);
    }

    //This is an AI mode. Should be triggered via a state machine in the main loop of an enemy
    public void GroundTrack(Vector3 targetPos)
    {
        if (!isAgent)
        {
            Debug.LogWarning("Non agent is attempting to use navmesh!", this);
            return;
        }

        if (nav.enabled == true && nav.isOnNavMesh)
        {
            nav.SetDestination(targetPos);
        }
        else if (!nav.isOnNavMesh)
        {
            nav.enabled = false;
        }


    }


    //Makes the enemy face the player
    public void LookAtPlayer()
    { 
        Vector3 transformOffset = player.transform.position + targetOffset;
        transform.LookAt(transformOffset);
    }

    //Intended to make the enemy face anything
    public void LookAtObject(Transform target)
    {
        transform.LookAt(target);
    }

    public void LookAtObject(Vector3 target)
    {
        transform.LookAt(target);
    }


    //Takes damage amount from health, trigger death if it's enough to kill
    //This will probably be an observer/ subject system eventually
    public void TakeDamage(int amount)
    {
        //Debug.Log(this.name + " took damage!");

        currentHealth -= amount;

        aiState = AIState.knockback;

        if (currentHealth <= 0)
        {
            StartCoroutine(OnDeathKnockBack());
        }
            

    }

    //Knocks back the enemy before killing them
    IEnumerator OnDeathKnockBack()
    {
        autoSwapAIState = false;
        aiState = AIState.idle;
        rigid.AddForce(transform.forward * -1 * onDeathKnockbackForce);
        yield return new WaitForSeconds(onDeathWait);
        EnemyDead(setDeath);
    }


    //Intended to force the enemy to avoid other enemies. This could be triggered after a certain amount of enemies are in range
    //This is defunct and very very old
    public void AvoidFlock(bool avoidVertically)
    {
        Collider[] touching = Physics.OverlapSphere(transform.position, transform.lossyScale.x);

        Vector3 awayVector = new Vector3();

        if (touching.Length != 0)
        {
            for (int i = 0; i < touching.Length; i++)
            {
                if (touching[i].gameObject != this.gameObject && touching[i].tag == "Enemy")
                {
                    //Debug.Log("touching " + touching[i].name);

                    awayVector += Vector3.MoveTowards(transform.position, touching[i].gameObject.transform.position, 0.1f);


                    if (!avoidVertically)
                        awayVector.y = transform.position.y;

                    transform.Translate(transform.position - awayVector);


                    //Debug.Log(awayVector);

                }

            }


        }


    }



    //Makes the enemy fire if they call this script and aren't already firing
    virtual public IEnumerator FireTimer()
    {
        isFiring = true;

        yield return new WaitForSeconds(fireRate);

        if (canFire)
        {
            GameObject currentProj = Fire(projectilePrefab, new Vector3(0, 0, 0), this.gameObject);

            Projectile projAbstract = currentProj.GetComponent<Projectile>();

            projAbstract.damage = damage;
            projAbstract.lifetime = projectileLifetime;

            projRigid = currentProj.GetComponent<Rigidbody>();
            projRigid.AddForce((transform.forward + targetOffset) * projectileSpeed);

            StartCoroutine("FireTimer");
        }
        else
        {
            isFiring = false;
        }
    }

    //enum AiState { spawn, idle, chase, dodge, circle, fire, extraState1, extraState2, extraState3 };
    //Chooses a state based on what aiState is set to
    public void RunStateMachine()
    {
        switch ((int)aiState)
        {
            case 0:
                SpawnBehaviour();
                break;
            case 1:
                IdleBehaviour();
                break;
            case 2:
                ChaseBehaviour();
                break;
            case 3:
                DodgeBehaviour();
                break;
            case 4:
                CircleBehaviour();
                break;
            case 5:
                FireBehaviour();
                break;
            case 6:
                ExtraStateHandler(aiState);
                break;
            case 7:
                ExtraStateHandler(aiState);
                break;
            case 8:
                ExtraStateHandler(aiState);
                break;
            case 9:
                KnockbackState();
                break;
            case 10:
                StunState();
                break;
        }

    }

    void SpawnBehaviour()
    {
        //Enables the navmesh agent slowly enough for the navmesh to catch up
        if (isAgent)
        {
            Invoke("NavEnabler", 0.1f);
        }

        //Puts the enemy into the main idle loop
        aiState = AIState.idle;

    }

    //Small hack to enable the navmesh agent by invoke
    void NavEnabler()
    {
        nav.enabled = true;
    }

    //Causes the enemy to think about what they're doing currently
    public virtual void IdleBehaviour()
    {
        if (autoSwapAIState)
        {
            ReEvaluateState();
        }
      
    }

    //Chases the player until they leave range or another state is triggered
    void ChaseBehaviour()
    {

        dodgeBox.isSensing = true;

        if (travelVertically)
        {
            if (pathOutput == PathOutput.found)
            {
                FollowPath();
            }
            else if (canDoPath)
            {
                //Setting the true to false here will result in the enemy using precision pathfinding
                FindPath(true, player.transform.position);
            }
        }
        else
        {
            GroundTrack(player.transform.position);
        }

        if (!lookVerticallyWhileChasing)
        {
            float zStore = transform.rotation.z;
            float xStore = transform.rotation.x;

            LookAtPlayer();

            transform.SetPositionAndRotation(transform.position, new Quaternion(xStore, transform.rotation.y, zStore, transform.rotation.w));
        }
        else
        {
            ///LookAtPlayer();
        }


        if (autoSwapAIState)
            ReEvaluateState();
    }

    //Makes the enemy dodge if canDodge is reset and the dodge chance is succesful
    void DodgeBehaviour()
    {
        if (isAgent)
        {
            nav.velocity = Vector3.zero;
        }

        dodgeBox.isSensing = false;

        //Debug.Log("ᕕ( ᐛ )ᕗ");


        if (canDodge)
        {
            int dodgeRand = Random.Range(0, 100);

            if (dodgeRand > dodgeChance)
            {
                aiState = AIState.idle;
                canDodge = false;
                StartCoroutine(ResetDodge());
                return;
            }
            else
            {
                canFire = false;
                //Debug.Log("Adding force");

                if (dodgeRand % 2 != 0)
                {
                    rigid.AddForce((transform.right * -1) * dodgeForce);
                    //Debug.Log("Right is righteous" + dodgeRand);
                }
                else
                {
                    rigid.AddForce(transform.right * dodgeForce);
                    //Debug.Log("Left is best" + dodgeRand);
                }

                StartCoroutine(ResetVelocity());
            }



        }

    }
    
    void CircleBehaviour()
    {
        //Go around the player to find a flank. Currently defunct
    }

    //Fire a projectile at the player
    //Triggers when canFire = true;
    public virtual void FireBehaviour()
    {
        dodgeBox.isSensing = true;

        //Debug.Log("Firing >:]");

        LookAtObject(pManager.enemyTarget);

        if (!isFiring)
            StartCoroutine(FireTimer());

        if (autoSwapAIState)
            ReEvaluateState();
    }

    //Think through whether we should swap our current state to something else
    public virtual void ReEvaluateState()
    {

        dodgeBox.isSensing = true;

        //Can we see the player and is he attacking and can we dodge and are we in a dodgeable state
        if (dodgeBox.isSeeingPlayer && (aiState == AIState.chase || aiState == AIState.fire || aiState == AIState.idle || aiState == AIState.circle))//Plus more states here if required
        {

            if (gameManager.isPlayerAttacking && canDodge)
            {
                //Debug.Log("Moving to dodge state");
                aiState = AIState.dodge;
                return;
            }


        }

        //Can fire will be set per enemy as to their firing conditions
        if (canFire)
        {
            aiState = AIState.fire;
            return;
        }

        //If the enemy can fly, it will always chase
        //If not, it will check if they're within valid chasing range
        if (travelVertically)
        {
            //Debug.Log("travel vertically is broken currently. Don't touch till it's fixed - Matt");
            aiState = AIState.chase;
            return;
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < chaseTolerance)
        {

            //Debug.Log(transform.position.y + " " + player.transform.position.y);
            aiState = AIState.chase;
            return;
        }


        //If nothing else is triggered, we go idle
        aiState = AIState.idle;

    }

    //Knocks the enemy back a little
    void KnockbackState()
    {
        //Debug.Log("knock knock");
        rigid.AddForce(transform.forward * -1 * knockbackForce);
        StartCoroutine(ResetVelocity());
    }

    //Forces inheritors to include a handler for extra states, if there are any
    public virtual void ExtraStateHandler(AIState aiState)
    {
        //For inheriting from
    }

    //Resets the enemy's velocity after it's been hit or dodging
    IEnumerator ResetVelocity()
    {
        canDodge = false;

        if (aiState == AIState.dodge)
        {
            yield return new WaitForSeconds(dodgeTime);
            nav.enabled = true;
        }
        else
        {
            yield return new WaitForSeconds(knockbackResetTime);
        }



        //Debug.Log("reset velocity");
        rigid.velocity = Vector3.zero;

        if (aiState == AIState.dodge)
        {
            StartCoroutine(ResetDodge());
        }

        if (autoSwapAIState)
        {
            aiState = AIState.idle;
        }


    }

    void StunState()
    {
        nav.velocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        if (!stunned)
        {
            stunned = true;
            StartCoroutine(StunWait());
        }
    }

    IEnumerator StunWait()
    {
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        aiState = AIState.idle;
    }


    //Resets the canDodge variable
    IEnumerator ResetDodge()
    {
        yield return new WaitForSeconds(dodgeWait);

        canDodge = true;
    }


    //Finds a path through the airnavmesh
    public virtual void FindPath(bool secondPass, Vector3 worldSpaceDestination)
    {
        //Clearing all lists
        occupiedNodes.Remove(storedNode);
        currentPath.Clear();
        openList.Clear();
        closedList.Clear();

        //Resetting pathing
        pathIndex = 0;
        endOfPath = false;

        if (runAway)
        {
            Ray ray = new Ray(transform.position,transform.forward * -1);

            worldSpaceDestination = ray.GetPoint(runAwayDistance);
           // Debug.Log(worldSpaceDestination);
           // Debug.Log(transform.position + "d");

            runAway = false;
        }


        //Setting our default destination
        destinationNode = currentNavMesh.nodes[0];

        //The closest node to us and the destiation are found
        for (int i = 0; i < currentNavMesh.nodes.Count; i++)
        {
            
            if(Vector3.Distance(transform.position,currentNode.worldPos) > nodeTolerance)
            {
                if (Vector3.Distance(currentNavMesh.nodes[i].worldPos, transform.position) < Vector3.Distance(currentNode.worldPos, transform.position))
                {
                    currentNode = currentNavMesh.nodes[i];
                }
            }
                
            
            if (Vector3.Distance(currentNavMesh.nodes[i].worldPos, worldSpaceDestination) < Vector3.Distance(destinationNode.worldPos, worldSpaceDestination) &&  !occupiedNodes.Contains(currentNavMesh.nodes[i]))
            {
                destinationNode = currentNavMesh.nodes[i];
            }
        }


        //Getting ready for the main loop by setting up lists
        pathOutput = PathOutput.processing;
        startNode = currentNode;
        closedList.Add(currentNode);
        storedNode = destinationNode;
        occupiedNodes.Add(destinationNode);

        //Stopping pathfinding if we're too far away
        if (Vector3.Distance(currentNode.worldPos, destinationNode.worldPos) < moveDistance)
        {
            canDoPath = false;
            pathOutput = PathOutput.inaccesible;
            StartCoroutine(PathResetter());
            return;
        }

        //Main loop
        for (int p = 0; p < searchRange; p++)
        {
            //Add all connected nodes from our current node to the open list
            AddConnectedToOpen(currentNode);

            Node nextNode = new Node();


            //Choose the closest node to our destination out of the current open list
            for (int i = 0; i < openList.Count; i++)
            {
                if (Vector3.Distance(openList[i].worldPos, destinationNode.worldPos) < Vector3.Distance(nextNode.worldPos, destinationNode.worldPos))
                {
                    nextNode = openList[i];
                }
            }

            //Add this node to the current path
            currentPath.Add(nextNode);

            //Close the node and remove it from open
            closedList.Add(nextNode);
            openList.Remove(nextNode);

            //Advance the current node
            currentNode = nextNode;


            //End pathfinding if our destination is found
            if (nextNode == destinationNode)
            {
                pathOutput = PathOutput.found;
                StartCoroutine(PathResetter());
                break;
            }

        }

        //If we didn't find the node, declare it inaccessible and reset pathfinding;
        if (pathOutput == PathOutput.processing)
        {
            pathOutput = PathOutput.inaccesible;

            if (canDoPath)
            {
                occupiedNodes.Remove(destinationNode);
                canDoPath = false;
                endOfPath = true;
                StartCoroutine(PathResetter());
            }


        }

        //If the parameter for a second path is true, we run an additional pathfinding algorithm
        if (secondPass && pathOutput == PathOutput.found)
        {
            RefinePathPass(closedList, startNode);
        }



    }

    //An additional pathfinding algorithm which works using already processed lists of nodes
    //Used to pathfind backwards from the goal towards the enemy through the already closed list. This gives a more precise path
    void RefinePathPass(List<Node> unknowns, Node destination)
    {
        unknowns = new List<Node>(unknowns.ToArray());

        currentPath.Clear();
        openList.Clear();
        closedList.Clear();



        pathOutput = PathOutput.processing;

        //Debug.Log(unknowns.Count);

        //Main loop - Mostly identical to the first
        for (int p = 0; p < unknowns.Count; p++)
        {

            AddConnectedToOpen(currentNode, unknowns);

            Node nextNode = new Node();

            for (int i = 0; i < openList.Count; i++)
            {
                if (Vector3.Distance(openList[i].worldPos, destination.worldPos) < Vector3.Distance(nextNode.worldPos, destination.worldPos))
                {
                    nextNode = openList[i];
                }
            }

            currentPath.Add(nextNode);

            //Debug.Log("Next node is" + nextNode.ID);

            closedList.Add(nextNode);
            openList.Remove(nextNode);

            currentNode = nextNode;


            if (nextNode == destination)
            {
                //Debug.Log("Found the goal but smarter 8]");
                pathOutput = PathOutput.found;
                break;
            }

        }

        //If we don't find it, cancel pathfinding
        if (pathOutput == PathOutput.processing)
        {
            //Debug.Log("Pathfinding could not find the way back from the destination. Something is very wrong. Call matt");

            if (canDoPath)
            {
                pathOutput = PathOutput.inaccesible;
                canDoPath = false;
                StartCoroutine(PathResetter());
            }
        }

        //Reverse the current path so that it is followed in the correct order
        currentPath.Reverse();

    }


    void AddConnectedToOpen(Node node)
    {
        //Add connected nodes from this node to the open list if they aren't already open or closed

        for (int i = 0; i < node.nodeList.InnerList.Count; i++)
        {
            if (!openList.Contains(currentNavMesh.nodes[node.nodeList.InnerList[i]]) && !closedList.Contains(currentNavMesh.nodes[node.nodeList.InnerList[i]]))
            {
                openList.Add(currentNavMesh.nodes[node.nodeList.InnerList[i]]);

            }
        }
    }

    //Add connected nodes from this node to the open list if they aren't already open or closed
    //Additionally checks if they're in the unknowns list too
    void AddConnectedToOpen(Node node, List<Node> unknowns)
    {
        for (int i = 0; i < node.nodeList.InnerList.Count; i++)
        {
            if (!openList.Contains(currentNavMesh.nodes[node.nodeList.InnerList[i]]) && !closedList.Contains(currentNavMesh.nodes[node.nodeList.InnerList[i]]) && unknowns.Contains(currentNavMesh.nodes[node.nodeList.InnerList[i]]))
            {
                openList.Add(currentNavMesh.nodes[node.nodeList.InnerList[i]]);
            }
        }
    }


    //Make the enemy move along the found path
    public virtual void FollowPath()
    {
        switch (pathOutput)
        {
            case PathOutput.found:

                try
                {
                    //Move us towards the next node on the path
                    transform.position = Vector3.MoveTowards(transform.position, currentPath[pathIndex].worldPos, moveSpeed);

                    //Look at the node we are going to
                    LookAtObject(currentPath[pathIndex].worldPos);
                }
                catch
                {
                    pathOutput = PathOutput.processing;
                }
                

                //If we're within nodetolerance, increment the path after checking for obstacles. If there are obstacles, it's the end of the path
                if (Vector3.Distance(transform.position, currentPath[pathIndex].worldPos) < nodeTolerance && pathIndex < currentPath.Count)
                {
                    RaycastHit[] hit;

                    try
                    {
                        hit = Physics.RaycastAll(currentPath[pathIndex].worldPos, (currentPath[pathIndex].worldPos - currentPath[pathIndex + 1].worldPos).normalized, Vector3.Distance(currentPath[pathIndex].worldPos, currentPath[pathIndex + 1].worldPos) + 0.1f);

                        for (int i = 0; i < hit.Length; i++)
                        {
                            if (hit[i].collider.gameObject.CompareTag("Hookable") || !hit[i].collider.gameObject.CompareTag("Enemy"))
                            {
                                //Found an obstacle
                                pathIndex = currentPath.Count - 1;
                                return;
                            }
                        }
                    }
                    catch
                    {

                    }

                    pathIndex++;

                }

                //If we're at the end of the path, stop everything and look at the player
                if (pathIndex == currentPath.Count - 1)
                {
                    endOfPath = true;
                    StartCoroutine("PathResetter");
                    pathOutput = PathOutput.processing;
                    transform.LookAt(player.transform);
                }
                else
                {
                    endOfPath = false;

                }

                break;
        }



    }

    IEnumerator PathResetter()
    {
        //Debug.Log("Path reset");
        pathIndex = 0;
        canDoPath = false;
        yield return new WaitForSeconds(refreshRate);//Random.Range(1,10));//refreshRate);

        canDoPath = true;
    }

    private void OnDestroy()
    {
        if (gameManager.enemyList.Contains(gameObject))
        {
            //gameManager.RemoveEnemyList(gameObject);
        }
    }



}