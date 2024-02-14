using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour {

    public float jumpForce;

    // Use this for initialization
    void Start () {
		
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<vp_FPController>().m_FallSpeed = jumpForce; //(Vector3.up * jumpForce);
        }
    }
}
