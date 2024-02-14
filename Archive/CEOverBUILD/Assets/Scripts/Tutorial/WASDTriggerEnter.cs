using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDTriggerEnter : MonoBehaviour {

    public GameObject wasdBarrier;
    public GameObject wasdBarrier2;
    public GameObject wasdBarrier3;

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
            wasdBarrier.SetActive(false);
            wasdBarrier2.SetActive(false);
            wasdBarrier3.SetActive(false);
        }
    }
}
