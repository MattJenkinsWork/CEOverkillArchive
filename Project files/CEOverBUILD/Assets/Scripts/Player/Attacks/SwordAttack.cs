using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{

    Animator anim;
    Collider capsuleCol;
    public GameObject sword;
    vp_FPController controller;
    WeaponManager wManager;

    //has the player clicked again during the specified period
    public bool hasClickedAgain;

    //Are we checking for additional clicks
    public bool isCheckingClicks;
    public bool isCheckingClicks2;

    //How long we'll wait for input while attacking
    public float waitPeriodForClicks;

    //Time when last button was clicked
    float lastClickedTime = 0;
    //Delay between clicks for which clicks will be considered as combo
    float maxComboDelay = 1;

    //Cooldown time between attacks (in seconds)
    public float cooldown = 0.5f;
    //Max time before combo ends (in seconds)
    public float maxTime = 0.8f;
    //Max number of attacks in combo
    public int maxCombo = 3;
    //Current combo
    public int combo = 0;
    //Time of last attack
    float lastTime;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        wManager = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponManager>();
        StartCoroutine("Melee");
        //anim.SetInteger("combo", 0);
        //capsuleCol = sword.GetComponent<CapsuleCollider>();
        controller = GetComponentInParent<vp_FPController>();
    }

    public void NotDrawing()
    {
        //REMOVE BEFORE COMPLETION
        //Placeholder to make the placeholder sword equip work
        wManager.isDrawing = false;

    }
    
    // Update is called once per frame
    void Update()
    {
        //anim.SetBool("Slash1bool", false);

        //Setting whether the player is attacking
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Slash1bool", true);
            anim.SetBool("AttackLock", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("Slash1bool", false);
        }


        if (Input.GetKeyDown(KeyCode.Space) && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack1/3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack2/3") &&!anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack3/3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerFall"))
        {
            anim.Play("PlayerFall");
        }

        if (controller.Grounded)
        {
            try
            {
                anim.SetBool("Grounded", true);
            }
            catch
            {

            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerFall"))
            {
                anim.Play("PlayerLand");
            }

            
        }
        else
        {
            anim.SetBool("Grounded", false);
        }

        //Sets to true if the player clicked again during the animation
        if (isCheckingClicks && Input.GetMouseButtonDown(0))
        {
            //Debug.Log("CLICKED");
            anim.SetTrigger("NextAnimationTrigger");
            //hasClickedAgain = true;
        }

        if (isCheckingClicks2 && Input.GetMouseButtonDown(0))
        {
            //Debug.Log("CLICKED");
            anim.SetTrigger("NextAnimationTrigger2");
            //hasClickedAgain = true;
        }
        //Debug.Log(isCheckingClicks);
        ////Enables and disables the collider based on whether we're attacking
        //if(anim.GetBool("Attacking"))
        //{
        //    capsuleCol.enabled = true;
        //}
        //else
        //{
        //    capsuleCol.enabled = false;
        //}
    }

    IEnumerator Melee()
    {
        //Constantly loops so you only have to call it once
        while (true)
        {

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
            {
                combo = 0;
            }
            

            //Checks if attacking and then starts of the combo
            if (Input.GetMouseButtonDown(0))
            {
                combo++;
                //anim.SetInteger("combo", combo);
                //Debug.Log("Attack" + combo);
                lastTime = Time.time;

                //Combo loop that ends the combo if you reach the maxTime between attacks, or reach the end of the combo
                while ((Time.time - lastTime) < maxTime && combo < maxCombo)
                {
                    //Attacks if your cooldown has reset
                    if (Input.GetMouseButtonDown(0) && (Time.time - lastTime) > cooldown)
                    {
                        if(combo < 3)
                        {
                            combo++;
                        }
                        
                        //anim.SetInteger("combo", combo);
                        //Debug.Log("Attack " + combo);
                        lastTime = Time.time;
                    }
                    yield return null;
                }
                //Resets combo and waits the remaining amount of cooldown time before you can attack again to restart the combo
                combo = 0;
                //anim.SetInteger("combo", combo);
                yield return new WaitForSeconds(cooldown - (Time.time - lastTime));
            }
            yield return null;
        }
    }


    public IEnumerator ClickChecker()
    {
        isCheckingClicks = true;
        //Debug.Log("Check for clicks");
        yield return new WaitForSeconds(waitPeriodForClicks);
        isCheckingClicks = false;

    }

    public IEnumerator ClickChecker2()
    {
        isCheckingClicks2 = true;
        //Debug.Log("Check for clicks");
        yield return new WaitForSeconds(waitPeriodForClicks);
        isCheckingClicks2 = false;

    }


    public void CheckForInput()
    {
        if (hasClickedAgain)
        {
            anim.SetTrigger("NextAnimationTrigger");
        }
        else
        {
            anim.SetBool("ResetIdleState", false);
        }
        StopAllCoroutines();
    }

    public void AnimationExitReset()
    {
        anim.SetBool("ResetIdleState", true);
    }

    public void AnimContReset()
    {
        anim.SetBool("RunNextAnim", false);
        anim.SetBool("ResetIdleState", false);
        isCheckingClicks = false;
        hasClickedAgain = false;
    }

    public void AttackLockOff()
    {
        anim.SetBool("AttackLock", false);
    }

}