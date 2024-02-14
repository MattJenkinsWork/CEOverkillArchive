using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;

public class SFX_Hooker : MonoBehaviour {


    public static SFX_Hooker instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (instance != null)
        {
            Destroy(this);
        }
    }


    //Once, when the floor destroys
    public void OnFloorDestroy(Vector3 floorPos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Level/floor_destroy1", floorPos);
    }

    //Plays the sound whenever a striker is hit, at the position specified
    public void OnStrikerDamaged(Vector3 strikerPos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/enemy_damaged", strikerPos);
    }

    //Plays whenever the gun is fired
    public void OnGunFire(Vector3 gunPos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player Character/LaserBlast", gunPos);
    }

    //Plays when the sword hits something
    //I can differentiate between different hits, if required
    public void playercharacter_weapon_hit1(Vector3 swordPos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player Character/Weapon/playercharacter_weapon_hit1", swordPos);
    }

    //Plays when flowerbots fire
    public void FlowerBot_Attacking(Vector3 flowerPos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/FlowerBot_Attacking", flowerPos);
    }

    //Plays when strikers attack
    public void OnStrikerAttack(Vector3 attackPos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Striker_Attacking", attackPos);
    }

    //                                                          |
    //Dunno if you want these, but they're here just in case    \/

    //Plays when strikers buildup a leap
    public void OnStrikerBuildup(Vector3 strikerPos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Striker_Chargeup", strikerPos);
    }

    //Plays when strikers land
    public void OnStrikerLand(Vector3 strikerPos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Striker_Attacking", strikerPos);
    }
    


    

}
