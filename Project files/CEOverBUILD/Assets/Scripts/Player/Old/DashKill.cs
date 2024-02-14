using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashKill : MonoBehaviour
{

    // Use this for initialization

    //AIBehaviour ai;

    void Start()
    {

      //  ai = GetComponent<AIBehaviour>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("hit");


        if (col.gameObject.tag == "Enemy")
        {
            //ai.health -= 100f;
            Destroy(col.gameObject);

        }
    }

}

