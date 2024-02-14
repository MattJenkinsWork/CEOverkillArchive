using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour {

    [Header ("Projectile Stats")]
    public float speed;
    public int damage;
    public Vector3 spawnOffset;
    public GameObject projectile;
    public float rechargeRate;
    public float rateOfExpansion;
    public float maxDistance;
    public int chargeCount;
    public int maxChargeCount;
    bool canCharge;

    Camera cam;
    public bool canFire = true;

    private void Start()
    {
        cam = transform.GetChild(0).GetComponent<Camera>();
        chargeCount = 3;
        canCharge = false;
    }

    // Update is called once per frame
    void Update () {

        if (chargeCount < maxChargeCount && canCharge == true)
        {
            canCharge = false;
            RechargeAttack();
        }

        if (chargeCount < maxChargeCount)
            canCharge = true;

        if (chargeCount > 0)
        {
            canFire = true;
        }
        else
        {
            canFire = false;
        }

        if (Input.GetKey(KeyCode.E) && canFire)
        {
            chargeCount -= 1;
            Projectile currentProj = Instantiate(projectile, cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,cam.nearClipPlane)) + spawnOffset, cam.transform.rotation).GetComponent<Projectile>();
            currentProj.speed = speed;
            currentProj.damage = damage;
            currentProj.gameObject.GetComponent<PlayerProjectile>().rateOfExpansion = rateOfExpansion;
            currentProj.gameObject.GetComponent<PlayerProjectile>().maxDistance = maxDistance;
            currentProj.gameObject.GetComponent<PlayerProjectile>().playerPos = transform.position;
            
        }
	}


    public void RechargeAttack() 
    {
        //Debug.Log("Function Started");
        StartCoroutine(WaitPeriod());
    }

    IEnumerator WaitPeriod()
    {
       // Debug.Log("Charge Time Started");
        yield return new WaitForSeconds(rechargeRate);
        chargeCount += 1;
       // Debug.Log("Charge Added");

    }

}
