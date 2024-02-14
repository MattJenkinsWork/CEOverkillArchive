using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
public class GameManager : MonoBehaviour {


    //Get all the player input scripts
    GrappleHook grappleController;
    DashLR dashController;
    WallRunning wallrunController;
    vp_FPController physicsController;
    SFX_Hooker sfx;
    //vp_FPInput inputController;
    
    //Grab the EndGameManager
    EndGameManager endGameController;

    //Player related stateistics
    [HideInInspector]
    public GameObject player;
    Animator playerAnim;
    [HideInInspector]
    public bool isPlayerAttacking;

    //What gravity is set to when not using any abilities
    public float gravityModifierOveride;

    //Minimum and Maximum Acceleration values for top and bottom speed limits
    public float maxSpeed;
    public float minSpeed;

    //A list of all the props that will be erased after a new floor begins
    [HideInInspector]
    public List<GameObject> propsToDestroy = new List<GameObject>();

    //Self explanatory
    [HideInInspector]
    public int floorsCleared = 0;

    //List of all the enemies alive right now
    [HideInInspector]
    public List<GameObject> enemyList = new List<GameObject>();

    //A variable that is set when AllEnemiesDeadCheck is called. This is independent from the spawn script variable of the same name
    [HideInInspector]
    public bool allEnemiesDead = false;

    public int maxFloorAmount = 14;

    //Other scripts within the GameManager object
    Spawn spawn;
    TowerDataHolder towerData;
    NavmeshManager navManager;
    SpawnSorter sorter;

    //If checked, the manager will trigger spawns
    public bool spawnEnemies;

    //If checked, the game can be won
    public bool canEnd = true;

    //Towertype defines what types of floors are spawned. Unfortunately will likely be unused
    public enum TowerType { normal = 1, water = 2, sky = 3 }
    [HideInInspector]
    public TowerType towerStore;

    //GameObject storage for destroying when the floor clears
    public GameObject currentFloorObject;


    //What height the enemies spawn at
    public float spawnHeight;

    //The lists of data taken from the floordata for this current floor
    [HideInInspector]
    public List<GameObject> enemyTypes;

    [HideInInspector]
    public List<GameObject> designateds;

    //How many enemies should be spawned
    [HideInInspector]
    public int numToSpawn;

    //can you attack in air right now?
    bool airAttack = true;
    
    //How long should the player be allowed to stay up for
    public float airTime;
    
    //How long before the player is able to use airTime again?
    public float airTimeReset;
    

    //Dirty singleton script
    static GameManager instance;

    //All to do with the tilefall script. Check that for more details!
    [Header("Floor Falling")]
    public float maxTimeToFall = 7;
    public float minTimeToFall = 5;
    public float destroyTime = 10;
    public float initialVelocity = 100;
    public float shaderUpdateRate = 0.015f;
    public float shaderDecreaseAmount = 0.01f;


    // Use this for initialization
    void Awake()
    {
        //Dirty singleton script
        if (instance == null)
            instance = this;

        if (instance != this)
            Destroy(this);
       
        //Sets the towertype. Will probably never change :[
        towerStore = TowerType.normal;

        //Getting all scripts needed to run
        player = GameObject.FindGameObjectWithTag("Player");
        towerData = GetComponent<TowerDataHolder>();
        spawn = GetComponent<Spawn>();
        navManager = GetComponent<NavmeshManager>();
        sorter = GetComponent<SpawnSorter>();
        grappleController = player.GetComponent<GrappleHook>();
        dashController = player.GetComponent<DashLR>();
        wallrunController = player.GetComponent<WallRunning>();
        physicsController = player.GetComponent<vp_FPController>();
        //inputController = player.GetComponent<vp_FPInput>();
        playerAnim = player.GetComponentInChildren<Animator>();
        endGameController = GetComponent<EndGameManager>();

        //Getting the current floor info
        sorter.GetSpawns();
        StartCoroutine(TagWait());

        //locks the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        if(SFX_Hooker.instance == null)
        {
            gameObject.AddComponent<SFX_Hooker>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If any of these abilities are being used, set gravity to 0, else turn it on
        if (grappleController.hooked == true || dashController.isDashing == true || wallrunController.IsWallrunning == true)
        {
            physicsController.PhysicsGravityModifier = 0.0f;

        }
        else
        {
            physicsController.PhysicsGravityModifier = gravityModifierOveride;

        }

        
        //Debug command for spawning floors
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            floorsCleared++;
            FloorController();
        }


        //Updates a bool to show if the player is attacking or not. This public bool can then be read from by enemies
        //HACK
        try
        {
            if (playerAnim.isActiveAndEnabled)
            {
                try
                {
                    if (playerAnim.GetBool("Attacking"))
                    {
                        //Debug.Log("Attack is happening");
                        isPlayerAttacking = true;

                        if (airAttack)
                        {
                            StartCoroutine(AirTime());
                            player.GetComponent<vp_FPController>().m_FallSpeed = 0.0f;
                        }

                    }
                    else
                        isPlayerAttacking = false;
                }
                catch
                {

                }
                
            }

            
        }
        catch
        {

        }

    }

    //Controls whether the player is currently attacking in the air or not
    IEnumerator AirTime()
    {
        airAttack = true;
        yield return new WaitForSeconds(airTime);
        airAttack = false;
        StartCoroutine(AirTimeReset());
    }

    //Resets the amount of time the player can spend in air
    IEnumerator AirTimeReset()
    {
        airAttack = false;
        yield return new WaitUntil((() => physicsController.Grounded));
        airAttack = true;
    }

    //Spawn calls this to add the enemy to this list
    public void AddEnemyList(GameObject enemy)
    {
        enemyList.Add(enemy);

    }

    //Enemies call this when they die
    public void RemoveEnemyList(GameObject enemy)
    {
        enemyList.Remove(enemy);

        spawn.CheckEnemyNumbers(enemy);

        EnemiesDeadCheck(true);

    }

    //Checks if there are still enemies alive. If the bool is set to true, the floor will be dropped
    public bool EnemiesDeadCheck(bool dropFloorIfTrue)
    {
        if (enemyList.Count == 0)
        {
            allEnemiesDead = true;
        }

        else
            allEnemiesDead = false;

        if (dropFloorIfTrue && allEnemiesDead && canEnd)
        {
            floorsCleared++;
            FloorController();
        }



        return allEnemiesDead;

    }

    //Controls how the floors spawn and the cleanup after each floor.
    public void FloorController()
    {

        if (floorsCleared < maxFloorAmount)
        {
            //Enables the next air nav mesh from the navmesh manager
            navManager.NextNavMesh();

            //Infuses all hextiles with the values here and then calls fall()
            for (int i = 0; i < currentFloorObject.transform.childCount; i++)
            {
                TileFall fall = currentFloorObject.transform.GetChild(i).GetComponent<TileFall>();
                fall.maxWaitTime = maxTimeToFall;
                fall.minWaitTime = minTimeToFall;
                fall.destroyTime = destroyTime;
                fall.initialVelocity = initialVelocity;
                fall.shaderDecreaseAmount = shaderDecreaseAmount;
                fall.shaderUpdateWait = shaderUpdateRate;
                fall.Fall();
            }

            SFX_Hooker.instance.OnFloorDestroy(currentFloorObject.transform.position);

            //Get rid of the remaining floor
            currentFloorObject.transform.DetachChildren();
            Destroy(currentFloorObject);

            Enemy.occupiedNodes = new List<Node>();

            //Make the next floor down appear
            towerData.currentFloors[floorsCleared].SetActive(true);

            //Wait a few seconds before getting new tags. Fixes an obscure bug to do with floor tags not being found
            StartCoroutine(TagWait());

            //Destroy all props
            foreach (GameObject prop in propsToDestroy)
            {
                Destroy(prop);
            }

            if (spawnEnemies)
            {
                spawn.ResetSpawns();
            }

        }
        else if (floorsCleared < maxFloorAmount + 1)
        {
            endGameController.startEndGame = true;
            endGameController.StartEnd();
        }

    }

   

    //Waits for everything to spawn, then gets relevant tags
    IEnumerator TagWait()
    {
        yield return new WaitForSeconds(0.1f);

        currentFloorObject = GameObject.FindGameObjectWithTag("Floor");
        ConvertArrayToList(propsToDestroy, GameObject.FindGameObjectsWithTag("ClearAfterFall"));
    }

    //A utility for converting a list of gameobjects to vector3s
    List<Vector3> ConvertGameObjectListToVector3(List<GameObject> gameObjects)
    {
        List<Vector3> vector3s = new List<Vector3>();

        if (gameObjects.Count == 0)
            Debug.LogError("FLOORDATA LACKS SPAWN POSITIONS OR POINTS OF INTEREST");
        foreach (GameObject gObject in gameObjects)
        {
            vector3s.Add(gObject.transform.localPosition);
            //Debug.Log(gObject.transform.Position);
        }

        return vector3s;

    }

    //Exactly what it says on the tin
    void ConvertArrayToList(List<GameObject> list, GameObject[] array)
    {
        foreach (GameObject arrayEntry in array)
        {
            list.Add(arrayEntry);
        }
    }

}
