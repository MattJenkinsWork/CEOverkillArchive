using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_FloorDestroy : MonoBehaviour
{

    public void floor_destroy1()
    {
        //FMOD_StudioSystem.instance.PlayOneShot(event:/Player Character/Weapon/playercharacter_weapon_swing1);
        //FMODUnity.RuntimeManager.instance.PlayOneShot("event:/Player Character/Weapon/playercharacter_weapon_swing1");

        //FMODUnity.RuntimeManager.PlayOneShot("event:/Level/floor_destroy1");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            floor_destroy1();
        }
    }
}