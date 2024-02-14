using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {

    //These variables are all set when fired
    public float speed;
    public int damage;
    public float lifetime = 10;
    public bool doesStun;

    [HideInInspector]
    public GameObject firedFrom;

    //Destroy function
    public abstract void Destructed();

    //Ticks down the lifetime and then kills the projectile
    public void LifetimeTick()
    {
        lifetime -= 1;

        if (lifetime <= 0)
            Destructed();

    }

    

    //Have damaging scripts and object pool scripts here

    //Maybe have a destruction mode too?

    //colour changes?




}
