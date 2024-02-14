using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAndLiftColliderAppear : MonoBehaviour {

    public GameObject finalText;
    public GameObject liftCollider;
    public GameObject liftCollider2;
    public GameObject liftCollider3;
    public GameObject liftCollider4;
    public GameObject liftCollider5;
    public GameObject liftCollider6;

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
            finalText.SetActive(true);
            liftCollider.SetActive(true);
            liftCollider2.SetActive(true);
            liftCollider3.SetActive(true);
            liftCollider4.SetActive(true);
            liftCollider5.SetActive(true);
            liftCollider6.SetActive(true);
        }
    }
}
