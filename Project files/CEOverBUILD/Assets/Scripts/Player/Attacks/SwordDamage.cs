using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour {

    public int damage = 0;
    public float dragWait;
    public float speedTaken;
    public float thrust;

    vp_FPController cont;

    GameObject player;

    public GameObject hitParticle;
    public GameObject hitPlace;

    Rigidbody enemyRigidbody;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cont = player.GetComponent<vp_FPController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit");

        if (other.gameObject.CompareTag("Enemy"))
        {
            

            SFX_Hooker.instance.playercharacter_weapon_hit1(other.transform.position);
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            GameObject particle = Instantiate(hitParticle, other.gameObject.transform.position, other.transform.rotation);
            particle.transform.SetParent(other.gameObject.transform);
            enemyRigidbody = other.GetComponent<Rigidbody>();
            enemyRigidbody.AddForce(transform.forward * thrust);
            if (!dragging)
            {
                //StartCoroutine(Drag());
            }

            
        }
        else if (other.gameObject.GetComponent<PropDestroy>())
        {
            other.gameObject.GetComponent<PropDestroy>().PropTakeDamage(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PropDestroy>() != null)
        {
            collision.gameObject.GetComponent<PropDestroy>().PropTakeDamage(1);
        }
    }

    bool dragging = false;

    IEnumerator Drag()
    {
        Debug.Log("Aaa");
        dragging = true;
        cont.MotorAcceleration -= speedTaken;

        if(cont.MotorAcceleration < GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().minSpeed)
        {
            cont.MotorAcceleration = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().minSpeed;
        }

        yield return new WaitForSeconds(dragWait);
        //cont.MotorAcceleration += speedTaken;
        dragging = false;
    }


    //THIS SCRIPT NEEDS A LOT OF CLEANING, IF IT'S EVER GONNA BE USED

    //SwordAttack swordScript;
    //Animator animParent;
    //public float freezeTime;

    //public GameObject manager;
    ////public GameManager gameManager;

    ////this is code that i (jose) added to prototype the sword doing more damage the more you move
    ////vp_FPController controller;
    ////public float playerSpeed;
    //public bool playerRunning;
    //public bool timerToStop;
    //public float damageModifier;
    //public float damageTimer;
    //private float timer;
    //public float damageCap;

    //public float speedIncreaseRate;
    //public float speedDecreaseRate;

    ////int that contains base damage
    //public int baseDamage;

    ////float where active damage is applied in formula, then converted into int for finalDamage
    //public float damageReturned;

    ////public int damageDone; not a need int rightnow

    //// The full output of damage that is done, after converting to int.
    //public int finalDamage;


    //public bool cameraShakeFlag1;
    //public bool cameraShakeFlag2;

    //public bool swordEquiped;

    ////Collider boxCollider;

    ////ignore
    //public bool okayToAttack;


    //public GameObject player;

    ////awake added for camera shake
    //void Awake()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    //gameManager = manager.GetComponent<GameManager>();
    //}


    //// Use this for initialization
    //void Start()
    //{
    //    //swordScript = GetComponentInParent<SwordAttack>();
    //    //animParent = GetComponentInParent<Animator>();
    //    controller = GetComponentInParent<vp_FPController>();
    //    swordEquiped = true;
    //    cameraShakeFlag1 = true;
    //    timer = damageTimer;
    //    //setting variables by jose
    //    playerRunning = false;
    //    damageModifier = 0;
    //    //damageTimer;
    //    timerToStop = false;
    //    damageCap = 6f;
    //    baseDamage = 1;
    //    //damageDone = 1;
    //    okayToAttack = false;
    // }

    /*
    //update made by jose, im trying to write it in a way even a dumbass like me can understand
    private void Update()
    {
        playerSpeed = controller.MotorAcceleration;


        //checks if player is moving
        if (Input.GetButton("Vertical") == true || Input.GetButton("Horizontal") == true)
        {
            
                if (controller.MotorAcceleration < gameManager.maxSpeed)
                {
                   // Debug.Log("Increasing Speed");
                    controller.MotorAcceleration += speedIncreaseRate;
                }
                else
                    controller.MotorAcceleration = gameManager.maxSpeed;


        } 
        else if (controller.MotorAcceleration >= gameManager.minSpeed)
        {
           // Debug.Log("Decreasing Speed");
            controller.MotorAcceleration -= speedDecreaseRate;


        }
        else
        {
            playerSpeed = 0f;
        }
        


        if (timer <= 0)
        {
            damageModifier += playerSpeed;
            timer = damageTimer;
        }
        else {
            timer -= Time.deltaTime;
        }

        //Debug.Log("damage mod is " + damageModifier);

        //StartCoroutine(ModifierCooldown());


    }
    */




    //void OnTriggerEnter(Collider col)
    //{
    //Just for now
    // if (col.gameObject.tag == "Enemy")
    // {
    //     col.GetComponent<Enemy>().TakeDamage(1);
    // }
    /*
            if (col.gameObject.tag == "Enemy")
            {



                /*
                //Debug.Log("Sword dealt damage");
                if (damageModifier >= 1f)
                {

                    while (damageModifier >= col.GetComponent<Enemy>().currentHealth && okayToAttack == false)
                    {

                        Debug.Log("Enemy health is " + col.GetComponent<Enemy>().currentHealth + " current damage modifier is " + damageModifier);
                        finalDamage = finalDamage + 1;
                        damageModifier = damageModifier - 1;
                        Debug.Log("final damage is " + finalDamage + " and damage modifier is " + damageModifier);

                        if (finalDamage == col.GetComponent<Enemy>().currentHealth)
                        {
                            okayToAttack = true;
                        }

                    }

                    okayToAttack = false;

                    col.GetComponent<Enemy>().TakeDamage(finalDamage);

                    finalDamage = 1;

                    /*
                    //damageDone = damageDone + 1;   this line was striked out during case playtesting
                    //Debug.Log("fuck 2 " + damageDone);


                    damageModifier = damageModifier - 1f;
                    col.GetComponent<Enemy>().TakeDamage(finalDamage);

                    //Debug.Log("damage fuck " + damageDone);
                    //damageDone = 1;                this line was striked out during case playtesting
                    */
    /*
}
else
{
    col.GetComponent<Enemy>().TakeDamage(baseDamage);
}

FMODUnity.RuntimeManager.PlayOneShot("event:/Enemyhit");

//if (cameraShakeFlag1 == true)
//{
//    cameraShakeFlag1 = false;
//    player.GetComponent<CameraShake>().enabled = true;
//    StartCoroutine(FreezeCooldown());
//}

////Debug.Log("damage fuck 3 " + damageDone);
////hit = true;
////if (!hit)
//  StartCoroutine(FreezePlayer());
}
else if (col.GetComponent<PropDestroy>() != null)
{
col.GetComponent<PropDestroy>().PropTakeDamage(1);
}
*/
    // }



    /*
    IEnumerator FreezePlayer()
    {
            //Debug.Log(Time.time);
            Time.timeScale = 1f;
            yield return new WaitForSecondsRealtime(freezeTime);
            Time.timeScale = 1f;
            StartCoroutine(FreezeCooldown());
    }

    IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<CameraShake>().enabled = false;
        cameraShakeFlag1 = true;
    }

    IEnumerator ModifierCooldown()
    {
        playerRunning = false;
        yield return new WaitForSecondsRealtime(damageTimer);
        timerToStop = false;
        Debug.Log("damage mod is " + damageModifier);
    }
    */
}
