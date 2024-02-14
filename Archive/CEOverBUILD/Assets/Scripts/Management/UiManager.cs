using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MapValues;

public class UiManager : MonoBehaviour {

    public float damageSpeed = 1;

    Image damageImage;
    Image shieldImage;
    Image recoverHealthImage;

    public GameObject arrowPrefab;

    GameObject canvas;

    //Relevant scripts
    PlayerManager pManager;
    GameManager gManager;
    vp_FPController pController;
    WeaponManager pWeapons;

    //Arrow stuff
    bool arrowActive = false;
    public float arrowLife = 5;

    //New order for canvas children
    //Pointer here
    Text ammo;
    Image speedBar;
    Image bulletFill;
    Image shieldBar;
    Image healthBar;
    Image warning;

    GameObject enemyCounterText;
    Text enemyCounter;

    GameObject deathText;
    GameObject restartText;
    Image ArrowPointN;

    int enemyCount;
    bool enemyCountActive = true;


    GameObject lastEnemyArrow;
    GameObject closestEnemy;

    void Start() {

        //Getting relevant scripts
        pManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        pController = pManager.gameObject.GetComponent<vp_FPController>();
        pWeapons = pManager.gameObject.GetComponent<WeaponManager>();
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        //Getting all the ui elements
        ammo = canvas.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        speedBar = canvas.transform.GetChild(1).GetChild(1).GetComponent<Image>();
        bulletFill = canvas.transform.GetChild(1).GetChild(2).GetComponent<Image>();
        shieldBar = canvas.transform.GetChild(1).GetChild(3).GetComponent<Image>();
        healthBar = canvas.transform.GetChild(1).GetChild(4).GetComponent<Image>();
        warning = canvas.transform.GetChild(1).GetChild(5).GetComponent<Image>();

        deathText = canvas.transform.GetChild(2).gameObject;
        restartText = canvas.transform.GetChild(3).gameObject;

        ArrowPointN = canvas.transform.GetChild(4).GetComponent<Image>();
        damageImage = canvas.transform.GetChild(5).GetComponent<Image>();
        shieldImage = canvas.transform.GetChild(6).GetComponent<Image>();
        recoverHealthImage = canvas.transform.GetChild(7).GetComponent<Image>();

        enemyCounterText = canvas.transform.GetChild(8).gameObject;
        enemyCounter = canvas.transform.GetChild(8).GetChild(0).GetComponent<Text>();

        lastEnemyArrow = canvas.transform.GetChild(11).gameObject;


        deathText.SetActive(false);
        restartText.SetActive(false);
        enemyCounterText.SetActive(false);

        //Small test to see if the canvas is in the correct order
        if (shieldBar == null)
        {
            Debug.LogError("WARNING: SOME OR ALL OF THE CURRENT CAVAS IS MISPLACED OR NOT UPDATED");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Shows dead text
        if (pManager.dead)
        {
            deathText.SetActive(true);
            restartText.SetActive(true);
        }






        enemyCount = gManager.enemyList.Count;


        if(enemyCount <= 10 && enemyCountActive)
        {
            enemyCounterText.SetActive(true);
            enemyCounter.text = (" " + enemyCount);

            lastEnemyArrow.SetActive(true);

        }
        else
        {
            enemyCounterText.SetActive(false);
            lastEnemyArrow.SetActive(false);
        }

        if (lastEnemyArrow.activeSelf)
        {

            closestEnemy = gManager.gameObject;

            for (int i = 0; i < gManager.enemyList.Count; i++)
            {
                if(Vector3.Distance(closestEnemy.transform.position, pManager.gameObject.transform.position) > Vector3.Distance(gManager.enemyList[i].transform.position, pManager.gameObject.transform.position))
                {
                    closestEnemy = gManager.enemyList[i];
                }

            }

            lastEnemyArrow.GetComponent<HitIndicator>().target = closestEnemy.transform.position;
        }


        if (enemyCount == 0)
        {
            //Debug.Log("dsa");
            enemyCounterText.SetActive(false);
            enemyCountActive = false;
            Invoke("EnemyCountResetter", 10);
        }


    }

    void EnemyCountResetter()
    {
        enemyCountActive = true;
    }



    //Shows a shield damage border
    public void ShieldDamage()
    {
        shieldImage.gameObject.SetActive(true);
        Invoke("RemoveFlash", damageSpeed);
    }

    //Shows a blood damage border
    public void DamageFlash()
    {
        damageImage.gameObject.SetActive(true);
        Invoke("RemoveFlash", damageSpeed);
    }

    //Removes all of the flashes
    public void RemoveFlash()
    {
        damageImage.gameObject.SetActive(false);
        shieldImage.gameObject.SetActive(false);
        recoverHealthImage.gameObject.SetActive(false);
    }

    //Shows a health gain border
    public void RecoverHealth()
    {
        recoverHealthImage.gameObject.SetActive(true);
        Invoke("RemoveFlash", damageSpeed);
    }

    GameObject arrow;

    //Creates a hit arrow on screen and sets it's target to the last enemy that hit the player
    public void HitIndicatorData(Vector3 hitPos)
    {
 
        if (arrowActive)
        {
            Destroy(arrow);
            arrowActive = false;
        }

        arrowActive = true;
        arrow = Instantiate(arrowPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        arrow.transform.SetParent(ArrowPointN.transform, false);
        arrow.transform.SetParent(canvas.transform);
        arrow.GetComponent<HitIndicator>().target = hitPos;
        Invoke("ArrowLife", arrowLife);
    }

    public void ArrowLife()
    {
        arrow.gameObject.SetActive(false);
    }





    //Updates the bars after everything else to avoid stuttering
    private void LateUpdate()
    {
        shieldBar.fillAmount = MapValuesExtension.Map(pManager.currentShields, 0, pManager.maxShields, 0, 1);
        healthBar.fillAmount = MapValuesExtension.Map(pManager.currentHealth, 0, pManager.maxHealth, 0, 1);
        speedBar.fillAmount = MapValuesExtension.Map(pController.MotorAcceleration, gManager.minSpeed, gManager.maxSpeed, 0, 1);
        bulletFill.fillAmount = pWeapons.ammo - (int)pWeapons.ammo;
   
        ammo.text = ((int)pWeapons.ammo).ToString();

        if (pManager.currentShields < 1)
        {
            warning.gameObject.SetActive(true);
        }
        else
            warning.gameObject.SetActive(false);

    }
}
