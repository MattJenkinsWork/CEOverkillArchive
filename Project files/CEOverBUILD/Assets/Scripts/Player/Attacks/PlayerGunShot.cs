using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunShot : Projectile {

    void Update()
    {
        //LifetimeTick();
    }


    public override void Destructed()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Enemy")
        {
            col.GetComponent<Enemy>().TakeDamage(damage);

            if (doesStun)
            {
                col.GetComponent<Enemy>().aiState = Enemy.AIState.stun;
            }

        }
        else if (col.GetComponent<PropDestroy>() != null)
        {
            col.GetComponent<PropDestroy>().PropTakeDamage(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destructed();
    }


}
