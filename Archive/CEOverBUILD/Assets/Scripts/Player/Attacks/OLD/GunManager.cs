using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour {

    [Header("Projectile Stats")]
    public float rateOfFire;
    public float projectileSpeed;
    public int damage;
    public GameObject projectilePrefab;
    public int projectileLifetime;
    public Vector3 fireOffset;
    public Vector3 targetOffset;
    public float ammo;
    public float regainRate = 0.1f;

    LineRenderer line;

    GameObject player;

    float nextFire = -1;

    [Header("Laser Stats")]
    bool charging;
    public GameObject laser;
    public float slowedAccelerationAmount = 0.5f;
    public float laserLifetime;
    public float laserRange;
    public int laserDamage;
    public float chargeTime = 2;
    public bool cheatyMode;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update () {


        if (Input.GetKey(KeyCode.W))
        {
            ammo += regainRate;
        }

        if (Input.GetKey(KeyCode.A))
        {
            ammo += regainRate;
        }

        if (Input.GetKey(KeyCode.S))
        {
            ammo += regainRate;
        }

        if (Input.GetKey(KeyCode.D))
        {
            ammo += regainRate;
        }

        if (Input.GetMouseButton(0))
        {
            FireCheck();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !charging)
        {
            ChargeAttack();
        }

        if (cheatyMode && Input.GetKey(KeyCode.Tab))
        {
            chargeTime = 0.001f;
            ChargeAttack();
        }

    }


    //Check if the object can fire
    public void FireCheck()
    {
        if (Time.time < nextFire)
            return;

        if(ammo > 1)
        {
            Fire();
        }


        nextFire = Time.time + rateOfFire;
    }


    void Fire()
    {
        Projectile bullet = Instantiate(projectilePrefab, transform.position + transform.forward + fireOffset, Quaternion.identity).GetComponent<Projectile>();
        bullet.lifetime = projectileLifetime;
        bullet.speed = projectileSpeed;
        bullet.damage = damage;

        ammo -= 1;

        bullet.gameObject.GetComponent<Rigidbody>().AddForce((transform.forward + targetOffset) * projectileSpeed);

    }

    public void ChargeAttack()
    {
        
        StartCoroutine(ChargeUp());
    }

    IEnumerator ChargeUp()
    {
        float accelerationStore = 0;
        charging = true;

        if (!cheatyMode)
        {
            accelerationStore = player.GetComponent<vp_FPController>().MotorAcceleration;
            player.GetComponent<vp_FPController>().MotorAcceleration = slowedAccelerationAmount;
        }
        
        yield return new WaitForSeconds(chargeTime);

        Projectile currentLaser = Instantiate(laser, transform.position /*- new Vector3(0,0,0.5f * laserRange)*/, transform.rotation).GetComponent<Projectile>();
        currentLaser.lifetime = laserLifetime;
        currentLaser.damage = laserDamage;
        currentLaser.GetComponent<PlayerLaser>().range = laserRange;

        Vector3[] laserPos = new Vector3[2];
        laserPos[0] = transform.position;
        laserPos[1] = currentLaser.transform.position;

        for (int i = 0; i < laserRange; i++)
        {
            laserPos[1] += transform.forward;
        }


        line.enabled = true;
        line.SetPositions(laserPos);
        Invoke("LaserReset", 0.2f);

        if (!cheatyMode)
        {
            player.GetComponent<vp_FPController>().MotorAcceleration = accelerationStore;
        }

        
        charging = false;
    }


    void LaserReset()
    {
        line.enabled = false;
    }
}
