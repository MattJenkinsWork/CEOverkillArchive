using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile {
    
    [HideInInspector]
    public float rateOfExpansion;
    [HideInInspector]
    public Vector3 playerPos;
    [HideInInspector]
    public float maxDistance;


    // Update is called once per frame
    void Update ()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.localScale += new Vector3(rateOfExpansion, 0, 0);

        if (Vector3.Distance(transform.position,playerPos) > maxDistance)
        {
            Destructed();
        }

    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Enemy")
        {
            col.GetComponent<Enemy>().TakeDamage(damage);
        }
        else if (col.GetComponent<PropDestroy>() != null)
        {
            col.GetComponent<PropDestroy>().PropTakeDamage(1);
        }
    }

    public override void Destructed()
    {
        Destroy(this.gameObject);
    }
}
