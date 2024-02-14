using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFloor : Enemy{

    //THIS SCRIPT IS DEFUNCT

    public override void ExtraStateHandler(AIState aI)
    {
        throw new System.NotImplementedException();
    }


    public override void DoDeathEffects()
    {
        gameManager.EnemiesDeadCheck(true);
    }


   /* private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(floor);
            Destroy(this.gameObject);
        }
    }*/
}
