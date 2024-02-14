using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy {

    [Header("Specific Stats")]
    public float fireDistance;
	
	// Update is called once per frame
	void Update () {

        //Manipulates the canFire value in order to make the state machine fire at the player
        if (Vector3.Distance(transform.position,player.transform.position) > fireDistance )
        {
            canFire = false;
        }
        else
        {
            canFire = true;
        }


        RunStateMachine();
    }
}
