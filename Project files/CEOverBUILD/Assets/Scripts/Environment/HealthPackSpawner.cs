using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour {

    public int[] floorIDArray = new int[5];

    private GameManager manager;

    public GameObject healthPack;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        HealthSpawnCheck();
    }


    void HealthSpawnCheck()
    {
        //Runs a loop and checks if this is an approved floor
        //Floors cleared updates as floors are completed
        for (int i = 0; i < floorIDArray.Length; i++)
        {
             if (floorIDArray[i] == manager.floorsCleared)
            {
                Instantiate(healthPack, transform.position,Quaternion.identity);
            }
        }
    }
}
