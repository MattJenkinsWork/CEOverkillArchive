using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

    CharacterController controller;

    public float footstepTimer;
    public bool timerToStop;
    public bool playerRunning;


	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        footstepTimer = 0.3f;
        timerToStop = false;
        playerRunning = false;
	}
	
	// Update is called once per frame
	void Update () {
		
        if (controller.isGrounded)
        {
            if (Input.GetButton("Vertical") == true || Input.GetButton("Horizontal") == true)
            {
                if (timerToStop == false)
                {
                    playerRunning = true;
                }
            }
        }

        if (playerRunning == true)
        {
            timerToStop = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player Character/Footsteps");
            StartCoroutine(FootstepTimer());
        }


	}

    IEnumerator FootstepTimer()
    {
        playerRunning = false;
        yield return new WaitForSeconds(footstepTimer);
        timerToStop = false;
    }
}
