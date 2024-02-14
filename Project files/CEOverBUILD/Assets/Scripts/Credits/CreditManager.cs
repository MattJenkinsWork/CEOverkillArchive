using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour {

    //Makes credits scroll upwards by moveamount every frame

    GameObject[] credits;
    public float moveAmount;

	// Use this for initialization
	void Awake()
    { 
        credits = GameObject.FindGameObjectsWithTag("Credit");
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach (GameObject credit in credits)
        {
            credit.transform.Translate(new Vector3(0, moveAmount * Time.deltaTime, 0));
        }
	}
}
