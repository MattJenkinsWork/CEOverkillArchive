using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour {

    //makes the object constantly point at the player


    GameObject player;

	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        float zStore = transform.rotation.z;
        float xStore = transform.rotation.x;

        transform.LookAt(player.transform);

        transform.SetPositionAndRotation(transform.position, new Quaternion(xStore, transform.rotation.y, zStore, transform.rotation.w));
    }
}
