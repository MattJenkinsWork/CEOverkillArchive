using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawning : MonoBehaviour {

    public List<GameObject> enemiesToSpawn;

    public int numberOfEnemies1;

    public int numberOfEnemies2;

    public int numberOfEnemies3;

    public int index;

    public bool stop;

    public float spawnWait;

    public float startWait;

    public int i;
    // Use this for initialization
    void Start () {
        stop = false;
        StartCoroutine(waitToSpawn()); 
    }
	
	// Update is called once per frame
	void Update ()
    {
        //for (i = index; i < 0; i--)
        //{
            
        //}
    }


    IEnumerator waitToSpawn()
    {
        Debug.Log("fuck2");
        //begins spawning new enemies
        yield return new WaitForSeconds(startWait);
        while (!stop)
        {
            Debug.Log("fuck");
            //randomly spawns one enemy at a time and waits a random amount of time before spawning another. Counts how many enemies have been spawned.

            //Finds the final spawn position
            Vector3 spawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

            //Debug.Log("Spawned enemy at " + spawnPosition );

            if (numberOfEnemies1 > 0)
            {
                GameObject enemy = (Instantiate(enemiesToSpawn[0], Vector3.zero, Quaternion.identity));
                enemy.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z);
                enemy.transform.localScale = Vector3.one;
                numberOfEnemies1 = numberOfEnemies1 - 1;
            }
            else if (numberOfEnemies1 == 0 && numberOfEnemies2 > 0)
            {
                GameObject enemy = (Instantiate(enemiesToSpawn[1], Vector3.zero, Quaternion.identity));
                enemy.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z);
                enemy.transform.localScale = Vector3.one;
                numberOfEnemies1 = numberOfEnemies2 - 1;
            }
            else if (numberOfEnemies1 == 0 && numberOfEnemies2 == 0 && numberOfEnemies3 > 0)
            {
                GameObject enemy = (Instantiate(enemiesToSpawn[2], Vector3.zero, Quaternion.identity));
                enemy.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z);
                enemy.transform.localScale = Vector3.one;
                numberOfEnemies1 = numberOfEnemies3 - 1;
            }
            else if( numberOfEnemies3 == 0 && numberOfEnemies2 == 0 && numberOfEnemies1 == 0)
            {
                stop = true;
            }
            /*
            GameObject enemy = (Instantiate(enemiesToSpawn[randomEnemy],manager.currentFloorObject.transform));
            enemy.transform.localPosition = new Vector3 (spawnPosition.x,spawnPosition.y + enemy.GetComponent<Enemy>().spawnOffset, spawnPosition.z);
            enemy.transform.SetParent(null);
            enemy.transform.localScale = Vector3.one;
            manager.AddEnemyList(enemy);
            */

            yield return new WaitForSeconds(spawnWait);
        }
    }
}
