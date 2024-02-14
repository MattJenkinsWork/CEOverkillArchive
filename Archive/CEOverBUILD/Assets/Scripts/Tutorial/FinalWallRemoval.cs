using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalWallRemoval : MonoBehaviour {

    public GameObject finalWall;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            finalWall.SetActive(false);

        }
    }
}
