using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerExplosion : MonoBehaviour {

    public int explosionDamage;

    public GameObject playerChar;

    public float destroyTime;


	// Use this for initialization
	void Awake () {
        playerChar = GameObject.FindGameObjectWithTag("Player");

	}

    private void Update()
    {
        if(transform.parent == null)
        {
            Invoke("DelayedDestroy", destroyTime);
        }


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerChar.GetComponent<PlayerManager>().PlayerTakeDamage(explosionDamage);
            DelayedDestroy();
        }
    }

    private void DelayedDestroy()
    {
        Destroy(gameObject);
    }
}
