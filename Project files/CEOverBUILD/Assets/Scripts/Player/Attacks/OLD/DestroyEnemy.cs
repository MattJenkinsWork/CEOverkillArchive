using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour {

    public GameObject lastEnemyTouched;
    public bool dead;
    public int enemiesCounted = 0;

	// Use this for initialization
	void Start () {

        dead = false;
	}
	

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" && lastEnemyTouched != col.gameObject)
        {
            dead = true;
            Destroy(col.gameObject);
            enemiesCounted++;
        }
        else
        {
            dead = false;
        }
    }  
}
