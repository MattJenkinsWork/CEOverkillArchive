using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugTrail : MonoBehaviour {

    public static bool playerEffects = false;
    public float slowDownEffect;
    public float playerSpeed;
    public float effectTime;

    private void Awake()
    {
        playerSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<vp_FPController>().MotorAcceleration;
    }
    public void OnTriggerEnter(Collider col)
    { 

       if(playerEffects == false && col.gameObject.tag == "Player")
       {
            GameObject.FindGameObjectWithTag("Player").GetComponent<vp_FPController>().MotorAcceleration = slowDownEffect;
            
            Debug.Log("Player Slowed");

            if (!playerEffects)
                StartCoroutine(RevertChanges());
       }
    }

    IEnumerator RevertChanges()
    {
        playerEffects = true;
        Debug.Log("Reverting");
        yield return new WaitForSecondsRealtime(effectTime);
        GameObject.FindGameObjectWithTag("Player").GetComponent<vp_FPController>().MotorAcceleration = playerSpeed;
        Debug.Log("Reverted");
        playerEffects = false;
    }
}
