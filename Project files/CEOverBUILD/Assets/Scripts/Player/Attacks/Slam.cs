using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slam : MonoBehaviour {

    vp_FPController cont;
    public float fallSpeed = 20;
    public bool isSlamming = false;
    public float radius = 5;
    public float heightBeforeSlam = 10;
    public int damage = 3;

    public bool canSlam = false;

    private void Awake()
    {
        cont = GetComponent<vp_FPController>();
    }

    // Update is called once per frame
    void Update () {

        if (!Physics.Raycast(transform.position, Vector3.down, heightBeforeSlam))
        {
            canSlam = true;
        }
        else
        {
            canSlam = false;
        }


        if (Input.GetKeyDown(KeyCode.F) && !cont.Grounded && canSlam)
        {
            cont.m_FallSpeed -= fallSpeed;
            isSlamming = true;
        }

        if (isSlamming && cont.Grounded)
        {
            FinishSlam();
        }

	}

    private void FinishSlam()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in hitObjects)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                col.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
        }

        isSlamming = false;

    }


}
