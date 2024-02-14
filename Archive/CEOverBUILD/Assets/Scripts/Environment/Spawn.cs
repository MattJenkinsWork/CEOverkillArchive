using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawn : MonoBehaviour
{
    //Should the spawn script use the generation system or not?
    [HideInInspector]
    bool useGenerationSystem = true;

    //Has the "LastEnemy" spawned?
    //As a note, this isn't used when the generation system is in effect
    [HideInInspector]
    public bool lastEnemySpawned = false;

    //Should the spawn system use the weights?
    bool useWeights;

    //Prefab for the lastEnemy
    [HideInInspector]
    public GameObject lastEnemy;

    [HideInInspector]
    TowerDataHolder towerData;

    //How long to wait before spawning an enemy
    public float spawnMostWait;
    public float spawnLeastWait;

    //variable that sets randomness in gap between enemies
    public float enemieRadius;

    //How many enemies have spawned
    [HideInInspector]
    public int enemiesSpawned = 0;

    //The max amount of enemies that can spawn
    public int maxEnemies;
    
    //How long to wait before initially spawning enemies
    public int startWait;

    //Has the spawning stopped?
    [HideInInspector]
    public bool stop;

    //Are all enemies dead?
    [HideInInspector]
    public bool allEnemiesKilled = false;

    GameManager manager;

    FloorData currentFloorData;

    public EnemySpawnProfile[] profiles;

    SpawnSorter sorter;

    public List<Vector3> globalSpawns;

    public int[] numberOfActiveEnemies;


    // Use this for initialization
    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sorter = manager.gameObject.GetComponent<SpawnSorter>();
        towerData = manager.gameObject.GetComponent<TowerDataHolder>();

        if (manager.spawnEnemies)
        {
            Invoke("ResetSpawns", 1);
            //ResetSpawns();
        }
        
    }

    public void ResetSpawns()
    {
        //Debug.Log("Reset span");
        StopCoroutine(waitToSpawn());
        enemiesSpawned = 0;
        maxEnemies = 0;

        //If the generation system is in effect, the values provided on the script are replaced with generation system values!
        //Nowadays, this should always be the case
        if (useGenerationSystem)
        {
            //Finding the current floor data, and sounding the alarm if it isn't found
            currentFloorData = towerData.tower1Data[manager.floorsCleared];

            try
            {
                int p = currentFloorData.amountOfEnemyToSpawn[0];
                if(p == 2)
                {
                    //Do nothing
                }
            }
            catch
            {
                Debug.LogError("CRITICAL ERROR: FLOOR DATA FOR FLOOR " + manager.floorsCleared + " COULD NOT BE FOUND / READ. PLEASE MAKE SURE THE FLOORDATA FOR THIS FLOOR IS CONTAINED IN TOWERDATAHOLDER AND EACH ARRAY HAS AT LEAST ZEROES IN IT OR TALK TO MATT");
                return;
            }



            //Creating new spawn profiles for the floor
            profiles = new EnemySpawnProfile[currentFloorData.enemyKinds.Length];

            for (int i = 0; i < currentFloorData.enemyKinds.Length; i++)
            {
                profiles[i] = new EnemySpawnProfile(currentFloorData.enemyKinds[i],
                                                    currentFloorData.amountOfEnemyToSpawn[i],
                                                    currentFloorData.maxAmountOfEnemyInField[i],
                                                    new List<Vector3>());
            }

            sorter.CreateSpawnList(manager.floorsCleared);
          
             //Adding unique spawn locations to each profile if the prefab is the same
             for (int i = 0; i < sorter.floorDesignated.Count; i++)
             {
                 if(sorter.floorDesignated[i] != null)
                 {
                     for (int p = 0; p < profiles.Length; p++)
                     {

                         if(sorter.floorDesignated[i].designatedEnemy.GetComponent<Enemy>().GetType() == profiles[p].prefab.GetComponent<Enemy>().GetType())
                         {
                             profiles[p].spawnLocations.Add(sorter.floorDesignated[i].transform.position);
                             break;
                         }
                     }
                 }
             }

             

            //Counting the number of enemies that should be spawned
            foreach (var profile in profiles)
            {
                maxEnemies += profile.spawnAmount;
            }

            if (maxEnemies < 1)
            {
                Debug.LogWarning("Amount of enemies in floordata is set to none. Did you mean to do this?");
            }



            
        }

        StartCoroutine(waitToSpawn());
    }

    // Update is called once per frame
    void Update()
    {
       

        // if the number of enemies spawned reaches the max number of enemies, it stops spawning new enemies
        if (maxEnemies <= enemiesSpawned)
        {
            stop = true;
        }
        else
        {
            stop = false;
        }

        //if new enemies have stopped spawning and the 'Last Enemy' has not already been spawned into the scene, it will instantite it 
        if (stop == true && lastEnemySpawned == false && manager.EnemiesDeadCheck(false)) 
        {
            floorTrigger();
        }

        if (manager.EnemiesDeadCheck(false))
        {
            allEnemiesKilled = true;

        }
    }

    void floorTrigger()
    {
        //instantiates the 'Last Enemy', which only spawns when all the enemeis in the scene have been killed. 'Last Enemy' allows the player to trigger the floor drop.
        //Instantiate(lastEnemy);
        lastEnemySpawned = true;
    }

    public void CheckEnemyNumbers(GameObject deadEnemy)
    {
        if(numberOfActiveEnemies != null)
        {
            for (int i = 0; i < profiles.Length; i++)
            {

                try
                {
                    if (profiles[i].prefab.GetComponent<Enemy>().GetType() == deadEnemy.GetComponent<Enemy>().GetType())
                    {
                        numberOfActiveEnemies[i]--;
                    }
                }
                catch
                {
                    Debug.Log("Dead enemy is not part of the current enemylist but that's fine I think");
                }

                
            }


        }
    }

    IEnumerator waitToSpawn()
    {
        float spawnWait = Random.Range(spawnLeastWait, spawnMostWait);
        numberOfActiveEnemies = new int[profiles.Length];

        

        //begins spawning new enemies
        yield return new WaitForSeconds(startWait);
        while (!stop)
        {
            //Need to get spawn location and enemy type. Also increment a counter for each type

            EnemySpawnProfile chosenProfile = new EnemySpawnProfile(null,0,0,new List<Vector3>());

            for (int i = 0; i < profiles.Length; i++)
            {
                Randomise:

                int rand = Random.Range(0,profiles.Length);

                if (profiles[rand].hasBeenChecked)
                {
                    goto Randomise;
                }

                if (numberOfActiveEnemies[rand] < profiles[rand].maxAmountOnField && profiles[rand].totalSpawned < profiles[rand].spawnAmount)
                {
                    chosenProfile = profiles[rand];
                    numberOfActiveEnemies[rand]++;
                    chosenProfile.totalSpawned++;
                    break;
                }
                else
                {
                    profiles[rand].hasBeenChecked = true;
                }
            }

            foreach (var prof in profiles)
            {
                prof.hasBeenChecked = false;
            }


            if(chosenProfile.prefab == null)
            {
                goto End;
            }


            Vector3 spawnPos = Vector3.zero;

            GameObject prefab = chosenProfile.Spawn(out spawnPos);

            //Debug.Log(prefab.name);

            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

         
            //Adds the enemy to the global list of enemies kept in the gamemanager. If the list is not empty, the floor will not drop
            manager.AddEnemyList(enemy);
            
            if (enemy.GetComponent<Enemy>().isAgent)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(enemy.transform.position, out hit, 5, NavMesh.AllAreas);
                //enemy.transform.position = hit.position;

                enemieRadius = Random.Range(1.5f, 2.5f);
                enemy.GetComponent<NavMeshAgent>().radius = enemieRadius;
            }
            else
            {
                enemy.transform.position = new Vector3(spawnPos.x, spawnPos.y + enemy.GetComponent<Enemy>().spawnOffset, spawnPos.z);
            }


            enemiesSpawned++;

            End:

            if (stop)
            {
                StopAllCoroutines();
            }
            else
            {
                yield return new WaitForSeconds(spawnWait);
            }
            
        }
    }
}

[System.Serializable]
public class EnemySpawnProfile
{
    public GameObject prefab;
    public int spawnAmount;
    public int totalSpawned = 0;
    public int maxAmountOnField;
    public List<Vector3> spawnLocations;

    [HideInInspector]
    public bool hasBeenChecked;

    public EnemySpawnProfile(GameObject enemyPrefab, int amountToSpawn, int maxSpawns, List<Vector3> spawnlocs)
    {
        prefab = enemyPrefab;
        spawnAmount = amountToSpawn;
        maxAmountOnField = maxSpawns;
        spawnLocations = spawnlocs;
    }


    public GameObject Spawn(out Vector3 spawnPos)
    {
        spawnPos = spawnLocations[Random.Range(0, spawnLocations.Count)];

        return prefab;
    }
    
}