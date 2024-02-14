using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimReciever : MonoBehaviour {

    //Recieves animation event signals from the gun animator

    WeaponManager wManager;

    public void Start()
    {
        wManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
    }

    public void NotDrawing()
    {
        wManager.isDrawing = false;
    }


    public void HolsterGun()
    {

    }

    public void HolsterSword()
    {

    }
}
