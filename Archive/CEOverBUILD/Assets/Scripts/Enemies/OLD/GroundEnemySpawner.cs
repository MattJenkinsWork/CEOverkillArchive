using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemySpawner : MonoBehaviour {

    public GameObject enemy;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
            Instantiate(enemy);

        if (Input.GetKey(KeyCode.Tab))
            Instantiate(enemy);
    }
}
