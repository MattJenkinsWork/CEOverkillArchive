using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamageAccDmg : MonoBehaviour
{

    //SwordAttack swordScript;
    //Animator animParent;
    public float freezeTime;

    //this is code that i (jose) added to prototype the sword doing more damage the more you move
    vp_FPController controller;

    public GameObject manager;
    public GameManager gameManager;

    public float playerSpeed;
    public bool playerRunning;
    public bool timerToStop;
    public float damageModifier;
    public float damageTimer;
    public float damageCap;
    public float speedIncreaseRate;
    public float speedDecreaseRate;
    public float backwardSpeed;

    //base damage as a int the player can do
    public int baseDamage;

    //float where active damage is applied in formula, then converted into int for finalDamage
    public float damageReturned;

    // The full output of damage that is done, after converting to int.
    public int finalDamage;


    public bool cameraShakeFlag1;
    public bool cameraShakeFlag2;

    //Collider boxCollider;



    public GameObject player;

    //awake added for camera shake
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = manager.GetComponent<GameManager>();
    }


    // Use this for initialization
    void Start()
    {
        //swordScript = GetComponentInParent<SwordAttack>();
        //animParent = GetComponentInParent<Animator>();
        //playerSpeed = controller.MotorAcceleration;
        controller = GetComponentInParent<vp_FPController>();
        cameraShakeFlag1 = true;

        //setting variables by jose
        playerRunning = false;
        damageModifier = 0;
        damageTimer = 0.25f;
        timerToStop = false;
        damageCap = 6f;
        controller.MotorAcceleration = gameManager.minSpeed;
        
    }

    //update made by jose, im trying to write it in a way even a dumbass like me can understand
    private void Update()
    {

        //checks if player is moving
        //if (Input.GetButton("Vertical") == true || Input.GetButton("Horizontal") == true)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (timerToStop == false)
            {
                if (controller.MotorAcceleration < gameManager.maxSpeed)
                {
                    //Debug.Log("Increasing Speed");
                    controller.MotorAcceleration += speedIncreaseRate * Time.deltaTime;
                }
                else
                    controller.MotorAcceleration = gameManager.maxSpeed;

            }
        }
        else if (Input.GetKey(KeyCode.S))
            controller.MotorAcceleration = backwardSpeed;
        else if (controller.MotorAcceleration >= gameManager.minSpeed)
        {
            //Debug.Log("Decreasing Speed");
            controller.MotorAcceleration -= speedDecreaseRate * Time.deltaTime;
        }


    }


    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Enemy")
        {
            DamageCalculation();
            Debug.Log(finalDamage);
            col.GetComponent<Enemy>().TakeDamage(finalDamage);

            FMODUnity.RuntimeManager.PlayOneShot("event:/Enemyhit");

            if (cameraShakeFlag1 == true)
            {
                cameraShakeFlag1 = false;
                player.GetComponent<CameraShake>().enabled = true;
                StartCoroutine(FreezeCooldown());
            }

            //Debug.Log("damage fuck 3 " + damageDone);
            //hit = true;
            //if (!hit)
            //  StartCoroutine(FreezePlayer());
        }
        else if (col.GetComponent<PropDestroy>() != null)
        {
            col.GetComponent<PropDestroy>().PropTakeDamage(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PropDestroy>() != null)
        {
            collision.gameObject.GetComponent<PropDestroy>().PropTakeDamage(1);
        }
    }

    void DamageCalculation()
    {
        if (controller.MotorAcceleration >= 0.3 && controller.MotorAcceleration <= 0.7)
            finalDamage = 0;
        else if (controller.MotorAcceleration >= 0.7001 && controller.MotorAcceleration <= 0.9)
            finalDamage = 1;
        else if (controller.MotorAcceleration >= 0.9001 && controller.MotorAcceleration <= 1.1)
            finalDamage = 2;
        else if (controller.MotorAcceleration >= 1.1001 && controller.MotorAcceleration <= 1.3)
            finalDamage = 3;
        else if (controller.MotorAcceleration >= 1.3001 && controller.MotorAcceleration <= 1.5)
            finalDamage = 4;
    }

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
        yield return new WaitForSeconds(damageTimer);
        timerToStop = false;
    }
}