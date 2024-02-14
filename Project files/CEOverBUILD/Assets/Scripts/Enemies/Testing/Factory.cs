using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Factory : Enemy {

    [Header("Factory Stats")]
    public GameObject[] enemies;
    public Vector3 spawnValues;
    public float spawnWait;
    public float mostWait;
    public float leastWait;
    public float playerDistance;
    public int startWait;
    public int enemiesSpawned;
    public int maxEnemies;
    public bool stop;
    int randomEnemy;
    public GameObject instantiated;
    public List<GameObject> enemiesInField;

    Animator anim;


    void Start ()
    {
        enemiesSpawned = 0;
        anim = transform.GetChild(2).GetComponent<Animator>();
        StartCoroutine(Startup());
	}
	
	void Update ()
    {
        spawnValues = transform.position;
        float store = transform.rotation.x;
        float storeagain = transform.rotation.z;
        LookAtPlayer();
        transform.SetPositionAndRotation(transform.position, new Quaternion(store, transform.rotation.y, storeagain, transform.rotation.w));



        // if the player is within a certain distance of the factory enemy, it will stop spawning new enemies and move away until it is a safe distance away
        if (playerDistance > Vector3.Distance(transform.position, player.transform.position))
        {
            Vector3 playerDirection = transform.position - player.transform.position;
            Vector3 newPos = transform.position + playerDirection;
            nav.SetDestination(newPos);
            StopCoroutine(SpawnEnemies());
        }


        // tracks to see how many enemies are in the field. If the number of enemies is the same as the max number of enemies, it will stop spawning more until that number goes down
        if (maxEnemies <= enemiesInField.Count)
        {
            stop = true;
        }
        else
        {
            stop = false;
        }

        spawnWait = Random.Range(leastWait, mostWait);

        AnimUpdate();
        
    }

    // triggers a start wait on spawning so enemies are not spawning in instantly
    IEnumerator Startup()
    {
        yield return new WaitForSeconds(startWait);
        StartCoroutine(SpawnEnemies());
    }

    // spawns enemies and adds it to a list so it can be used to track how many are in the field
    IEnumerator SpawnEnemies()
    {
        if (!stop)
        {
            randomEnemy = Random.Range(0, enemies.Length);
            instantiated = Instantiate(enemies[randomEnemy], transform.position, gameObject.transform.rotation); // leave spawn Values at 0, 0, 0 if enemies are going to spawn where the factory is
            instantiated.GetComponent<Enemy>().factory = gameObject;
            enemiesSpawned++;
            enemiesInField.Add(instantiated);
        }

        yield return new WaitForSeconds(spawnWait);

        StartCoroutine(SpawnEnemies());
    }

    //Abstracts used for additional states and fancy death stuff. DO NOT REMOVE
    public override void DoDeathEffects()
    {
        foreach (GameObject enemies in enemiesInField)
        {
            Destroy(enemies);
        }
    }

    void AnimUpdate()
    {
        anim.SetBool("isMoving", false);

        if(nav.velocity.magnitude > 0.001f)
        {
            anim.SetBool("isMoving", true);
        }

    }

    
}

