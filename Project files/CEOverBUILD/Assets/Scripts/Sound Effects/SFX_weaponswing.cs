using UnityEngine;
using System.Collections;

public class SFX_weaponswing : MonoBehaviour { 

	public void WeaponSwing1()
	{
        //FMOD_StudioSystem.instance.PlayOneShot(event:/Player Character/Weapon/playercharacter_weapon_swing1);
        //FMODUnity.RuntimeManager.instance.PlayOneShot("event:/Player Character/Weapon/playercharacter_weapon_swing1");
   
        FMODUnity.RuntimeManager.PlayOneShot("event:/Player Character/Weapon/WeaponSwing1");

        //Debug.Log("fuck");
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            WeaponSwing1();
        }
    }
}
