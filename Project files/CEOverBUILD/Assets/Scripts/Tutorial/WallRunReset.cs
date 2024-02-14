using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunReset : MonoBehaviour {

    public GameObject player;
    public GameObject respawn;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            vp_PlayerEventHandler player = other.transform.root.GetComponentInChildren<vp_PlayerEventHandler>();
            player.Position.Set(respawn.transform.position);
            player.Rotation.Set(respawn.transform.eulerAngles);

            //player.transform.position = respawn.transform.position;
            //player.transform.rotation = respawn.transform.rotation;

        }
    }
}
