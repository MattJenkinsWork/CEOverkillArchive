using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MapValues;

public class WeaponManager : MonoBehaviour {

    public enum Weapon { sword, gun };
    GameManager gManager;

    [Header ("Player Bits")]
    public Weapon currentWeapon;
    public GameObject gunArms;
    public GameObject gun;
    public GameObject sword;
    //public GameObject swordCapsuleCollider;
    public GameObject swordArms;
    public ParticleSystem swordTrail;
    public ParticleSystem swordGlow;
    public ParticleSystem swordSparks;
    vp_FPController controller;
    LineRenderer line;
    GameObject player;


    [Header ("Speed Stats")]
    public float speedIncreaseRate;
    public float speedDecreaseRate;
    float playerSpeed;
    public float gunSpeedDecrease;
    float accelSave;

    [Header("Gun Stats")]
    public float rateOfFire;

    public float projectileSpeed;
    public int gunDamage;
    public GameObject projectilePrefab;
    public int projectileLifetime;
    public Vector3 fireOffset;
    public Vector3 targetOffset;
    public float ammo;
    public float regainRate = 0.1f;
    float nextFire = -1;
    Camera fpsCam;
    public bool gunFireRateDamageScale;
    public float maxRateOfFire;
    public float minRateOfFire;
    public float maxGunDam;
    public float minGunDam;
    public bool stunGun;

    [Header("Hitscan Stats")]
    public bool isHitscan;
    public float hitScanRange;


    [Header("Laser Stats")]
    public GameObject laser;
    bool charging;
    public float slowedAccelerationAmount = 0.5f;
    public float laserLifetime;
    public float laserRange;
    public int laserDamage;
    public float chargeTime = 2;
    public bool cheatyMode;

    [Header("Sword Stats")]
    public int maxSwordDamage;
    public int minSwordDamage;
    Animator swordAnim;
    public Collider swordCol;
    int swordDamage;
    public bool damageBelowMax;

    //are we currently pulling out or drawing a weapon
    public bool isDrawing;

    //has the player clicked again during the specified period
    bool hasClickedAgain;

    //Are we checking for additional clicks
    bool isCheckingClicks;

    //How long we'll wait for input while attacking
    public float waitPeriodForClicks;

    Animator gunAnim;



    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        line = gun.GetComponent<LineRenderer>();
        swordAnim = swordArms.GetComponent<Animator>();
        //swordCol = swordCapsuleCollider.GetComponent<CapsuleCollider>();
        controller = player.GetComponent<vp_FPController>();
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        fpsCam = GetComponentInChildren<Camera>();
        gunAnim = gunArms.GetComponent<Animator>();


       // Debug.Log(controller.MotorAcceleration);


    }
	
	// Update is called once per frame
	void Update () {

        //Allows weapon swapping
        if (Input.GetKeyDown(KeyCode.Q) && !isDrawing)
        {
            if(currentWeapon == Weapon.gun)
            {
                swordAnim.Play("SwordDraw(NotImplemented)");
                //swordAnim.SetTrigger("ExitAnim");
                swordArms.transform.localPosition += Vector3.up * 100000;
                //isDrawing = true;
                currentWeapon = Weapon.sword;
            }
            else
            {
                swordArms.transform.localPosition += Vector3.up * 100000 * -1;
                currentWeapon = Weapon.gun;
                //isDrawing = true;
            }

           


        }


        //checks if player is moving
        if (Input.GetButton("Vertical") == true || Input.GetButton("Horizontal") == true)
        {
            ammo += regainRate * Time.deltaTime;
            swordAnim.SetBool("Running", true);

            if(currentWeapon == Weapon.gun)
            {
                gunAnim.SetBool("Running", true);
            }
            

            if(currentWeapon == Weapon.gun)
            {
                if (!gunFireRateDamageScale)
                {
                    if (controller.MotorAcceleration < (gManager.maxSpeed - gunSpeedDecrease))
                    {
                        // Debug.Log("Increasing Speed");
                        controller.MotorAcceleration += speedIncreaseRate * Time.deltaTime;
                    }
                    else
                        controller.MotorAcceleration = (gManager.maxSpeed - gunSpeedDecrease);
                }
                else
                {
                    if (controller.MotorAcceleration < gManager.maxSpeed)
                    {
                        // Debug.Log("Increasing Speed");
                        controller.MotorAcceleration += speedIncreaseRate * Time.deltaTime;
                    }
                    else
                        controller.MotorAcceleration = gManager.maxSpeed;
                }

                
            }
            else
            {
                if (controller.MotorAcceleration < gManager.maxSpeed)
                {
                    // Debug.Log("Increasing Speed");
                    controller.MotorAcceleration += speedIncreaseRate * Time.deltaTime;
                }
                else
                    controller.MotorAcceleration = gManager.maxSpeed;
            }

            

        }
        else if (controller.MotorAcceleration >= gManager.minSpeed)
        {
            // Debug.Log("Decreasing Speed");
            controller.MotorAcceleration -= speedDecreaseRate * Time.deltaTime;
            swordAnim.SetBool("Running", false);

            if(currentWeapon == Weapon.gun)
            {
                gunAnim.SetBool("Running", false);
            }

            

        }

        var emissionGlow = swordGlow.emission;
        emissionGlow.rateOverTime = MapValuesExtension.Map(controller.MotorAcceleration, gManager.minSpeed, gManager.maxSpeed, 0, 100);
        var emissionTrail = swordTrail.main;
        emissionTrail.maxParticles = Mathf.RoundToInt(MapValuesExtension.Map(controller.MotorAcceleration, gManager.minSpeed, gManager.maxSpeed, 10, 500));
        var emissionSparks = swordSparks.emission;
        emissionSparks.rateOverTime = MapValuesExtension.Map(controller.MotorAcceleration, gManager.minSpeed, gManager.maxSpeed, 0, 100);


        switch (currentWeapon)
        {
            case Weapon.sword:
                {
                    SwordUpdate();
                    break;
                }
            case Weapon.gun:
                {
                    GunUpdate();
                    break;
                }
        }
	}

    void SwordUpdate()
    {
        swordAnim.SetBool("isDrawing", isDrawing);

        //Debug.Log("We BE runnin");
        try
        {
            //sword.SetActive(true);

            gun.SetActive(false);
            gunArms.SetActive(false);
        }
        catch
        {
           // Debug.Log("CAUGHT");
        }

        if (damageBelowMax)
        {
            sword.GetComponent<SwordDamage>().damage = (int)MapValuesExtension.Map(controller.MotorAcceleration, gManager.minSpeed, gManager.maxSpeed, minSwordDamage, maxSwordDamage);
        }
        else if(controller.MotorAcceleration == gManager.maxSpeed)
        {
            sword.GetComponent<SwordDamage>().damage = maxSwordDamage;
        }
        else
        {
            sword.GetComponent<SwordDamage>().damage = 0;
        }


        //swordAnim.SetBool("Slash1bool", false);

        //Debug.Log(sword.GetComponent<SwordDamage>().damage);
        //
        //Dealing with Animations
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("It's true yay");
            swordAnim.SetBool("Slash1bool", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swordAnim.SetBool("Slash1bool", false);
        }

        //Sets to true if the player clicked again during the animation
        if (isCheckingClicks && Input.GetMouseButtonDown(0))
        {
            swordArms.GetComponent<SwordAttack>().hasClickedAgain = true;
        }

        if (!swordAnim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack1/3") && !!swordAnim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack3/3"))
        {
            swordAnim.SetBool("Attacking",false);
        }

        //Enables and disables the collider based on whether we're attacking
        if (swordAnim.GetBool("Attacking"))
        {
            swordCol.enabled = true;
            swordTrail.Play();

        }
        else
        {
            swordTrail.Stop();
            swordCol.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageBelowMax && other.gameObject.CompareTag("Enemy") && controller.MotorAcceleration == gManager.maxSpeed)
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(maxSwordDamage);
        }
    }

    void GunUpdate()
    {
        try
        {
            //sword.SetActive(false);
            
            gun.SetActive(true);
            gunArms.SetActive(true);
        }
        catch
        {

        }

        if (gunFireRateDamageScale)
        {
            rateOfFire = MapValuesExtension.Map(controller.MotorAcceleration, gManager.minSpeed, gManager.maxSpeed, minRateOfFire, maxRateOfFire);
            gunDamage = (int)MapValuesExtension.Map(controller.MotorAcceleration, gManager.maxSpeed, gManager.minSpeed, maxGunDam, minGunDam);
        }

        if (Input.GetMouseButton(0))
        {
            FireCheck();
        }
        else
        {
            gunAnim.SetBool("isShooting",false);
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

        Debug.DrawRay(Camera.main.ScreenToWorldPoint(new Vector3(0.5f, 0.5f)),fpsCam.transform.forward,Color.green);

        if (controller.Grounded)
        {
            gunAnim.SetBool("Grounded", true);
        }
        else
        {
            gunAnim.SetBool("Grounded", false);
        }

    }

    //Check if the object can fire
    public void FireCheck()
    {
        if (Time.time < nextFire)
            return;

        if (ammo > 1)
        {
            Fire();
            gunAnim.SetBool("isShooting", true);
        }


        nextFire = Time.time + rateOfFire;
    }


    void Fire()
    {
        SFX_Hooker.instance.OnGunFire(transform.position);


        if (!isHitscan)
        {
            Projectile bullet = Instantiate(projectilePrefab, gun.transform.position + (gun.transform.forward * 2.7f) + transform.right * 2.6f + transform.up * 0.5f + fireOffset, Quaternion.identity).GetComponent<Projectile>();
            bullet.lifetime = projectileLifetime;
            bullet.speed = projectileSpeed;
            bullet.damage = gunDamage;

            if (stunGun)
            {
                bullet.doesStun = true;
                bullet.damage = 0;
            }


            ammo -= 1;

            bullet.gameObject.GetComponent<Rigidbody>().AddForce((fpsCam.transform.forward + targetOffset) * projectileSpeed);
            bullet.gameObject.transform.LookAt(fpsCam.transform.forward * 10 + fpsCam.transform.position);
            
            return;
        }

        RaycastHit hit;
        Ray ray = new Ray();
        ray.origin = fpsCam.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0)).origin + fpsCam.gameObject.transform.forward;
        ray.direction = fpsCam.transform.forward;



        if (Physics.Raycast(ray, out hit, hitScanRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal) && ammo > 1)
        {
            //Debug.Log(hit.collider.gameObject);

            ammo--;

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(gunDamage);
            }
            else if (hit.collider.gameObject.GetComponent<DodgeBox>())
            {
                hit.collider.transform.GetComponentInParent<Enemy>().TakeDamage(gunDamage);
            }
            else if (hit.collider.gameObject.GetComponent<PropDestroy>())
            {
                hit.collider.gameObject.GetComponent<PropDestroy>().PropTakeDamage(1);
            }

            line.enabled = true;

            Vector3[] shotPos = new Vector3[2];
            shotPos[0] = gun.transform.position;
            shotPos[1] = hit.point;

            line.SetPositions(shotPos);
            Invoke("LaserReset", 0.1f);

        }
        

    }

    void SwordAnimUpdate()
    {
        //if()
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
