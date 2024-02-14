using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffects : MonoBehaviour
{
    //MOSTLY DEFUNCT FOR NOW, WILL AFFECT ENEMIES HIT IN FUTURE


    //Collider capsuleCol;
    public GameObject particleFX;
    Rigidbody enemyRB;
    public float explosiveForce;
    public float explosiveRadius;

    // Use this for initialization
    void Start()
    {
       // capsuleCol = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {

        //DISABLED FOR NOW

        /*
        enemyRB = collision.gameObject.GetComponent<Rigidbody>();
        if (enemyRB != null)
        {
            enemyRB.AddExplosionForce(explosiveForce, collision.contacts[0].point, explosiveRadius);
        }
        Debug.Log(collision.contacts[0].point);
        */


        //foreach (ContactPoint contact in collision.contacts)
        //{
        //   // print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
        //    // Visualize the contact point
        //    Debug.DrawRay(contact.point, contact.normal, Color.white);
            
            
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    enemyRB = other.gameObject.GetComponent<Rigidbody>();
    //    enemyRB.AddExplosionForce(explosiveForce, other.contacts[0].point, explosiveRadius);
    //}
}