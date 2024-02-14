using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLiftLight : MonoBehaviour {

    public float moveSpeed;

    private Vector3 startPos;

    public float wait;

    public float lowerBound;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.y > lowerBound)
        {
            transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0));
        }
        else
            transform.position = startPos;


	}
}
