using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    [Header("Health")]
    public int currentHealth;
    public int maxHealth;
    public int numOfBars;
    public bool dead;
    public bool damaged;

    public bool toxicDamage = false;

    public bool tutorial = false;

    [Header("Shields")]
    public int maxShields;
    public int currentShields;

    //How long before regen commences
    public int timeUntilShieldRegenerates;

    public int amountRegenerated;
    float timeToBeginRegen;
    bool shieldTimeReset;
    bool doShieldRegen;


    [Header("Invincibility")]

    public float invincibleTime;
    bool isInvincible = false;


    GameManager gameManager;

    [HideInInspector]
    public Vector3 enemyTarget;

    private void Awake()
    {
       gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
       currentShields = maxShields;
       dead = false;
       damaged = false;
    }

    void Update()
    {
        
        //For testing damage
        if (Input.GetKeyDown(KeyCode.Keypad1))
            PlayerTakeDamage(1);

        //For testing damage
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            maxHealth = 100000;
            PlayerRecoverDamage(100000);
        }

        //if (toxicDamage == true)
        //{
        //    Debug.Log("ToxicBool True");
        //    InvokeRepeating("ToxicDamage", 1f, 1f);

        //}


        //Update the shields if we're not dead
        if (!dead)
        {
            ShieldTick();
        }

        //Resetting the scene
        if (dead == true && tutorial == true & Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (dead == true & Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    public void PlayerTakeDamage(int amount)
    {
        if (!isInvincible)
        {
            StartCoroutine(Invincibility());
        }
        else
        {
            return;
        }

        //Remove from shield if they aren't empty
        if (currentShields != 0)
        {
            currentShields -= amount;
            gameManager.GetComponent<UiManager>().ShieldDamage();
        }
        else
        {
            currentHealth -= amount;
            gameManager.GetComponent<UiManager>().DamageFlash();
        }

        //Resets the shield regen time
        shieldTimeReset = true;
        doShieldRegen = false;
        damaged = false;

        //Freezes the player if they're dead
        if (currentHealth <= 0)
        {
            FreezeControls();
            //Debug.Log("You should be dead");
        }

    }

    IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }


    public void PlayerRecoverDamage(int healthAmount)
    {
        currentHealth += healthAmount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        gameManager.GetComponent<UiManager>().RecoverHealth();
    }

    public void ShieldTick()
    {
        //If active, this will reset the time it takes to regen shields
        if (shieldTimeReset)
        {
            timeToBeginRegen = Time.timeSinceLevelLoad + timeUntilShieldRegenerates;
        }

        //Do regen if we can
        if (Time.timeSinceLevelLoad > timeToBeginRegen)
        {
            
            doShieldRegen = true;
            
        }
        else
        {
            shieldTimeReset = false;
        }

        //Do regen if all is ok and an arbritrary number is even
        if (doShieldRegen && currentShields < maxShields && Time.timeSinceLevelLoad % 2 != 0)
        {
            //Debug.Log("Doing regen");
            currentShields += amountRegenerated;
        }

        if (currentShields > maxShields)
        {
            currentShields = maxShields;
        }
    }

    //Freeze everything and "kill" the player
    public void FreezeControls()
    {
        
        Time.timeScale = 0f;
        dead = true;
    }

    

    public IEnumerator ToxicDamage()
    {
        while (toxicDamage == true)
        {
            //Debug.Log("OnCoroutine: " + (int)Time.time);
            yield return new WaitForSeconds(1f);
            PlayerTakeDamage(1);
        }

    }
}

