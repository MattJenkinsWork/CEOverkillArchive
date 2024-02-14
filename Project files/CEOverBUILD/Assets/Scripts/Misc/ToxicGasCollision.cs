using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ToxicGasCollision : MonoBehaviour {

    public PostProcessingProfile toxicGasProfile;
    public PostProcessingProfile normalProfile;
    public PostProcessingBehaviour ppp;
    public PlayerManager pManager;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pManager.toxicDamage = true;
            StartCoroutine(pManager.ToxicDamage());  
            ppp.profile = toxicGasProfile;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pManager.toxicDamage = false;
            ppp.profile = normalProfile; 
        }

    }
}
