using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTrack : Projectile {

    //Ticks the lifetime counter till it dies
    private void Update()
    {
        LifetimeTick();
    }

    
    public void TurnMissile()
    {
        //Turn the missile here

    }

    //Destroy if we hit something solid
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != firedFrom)
            Destructed();
    }


    public override void Destructed()
    {
       Destroy(this.gameObject);
    }

    //Get the trigger we passed through and deal damage if it's the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerManager>().PlayerTakeDamage(damage);
            GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<UiManager>().HitIndicatorData(transform.position);
            Destructed();
        }
            


        
    }

}

/* transform.position = Vector3.MoveTowards(transform.position, target, speed);
       Debug.Log(target);

        if (transform.position == target)
            Destroy(this.gameObject);
       */
