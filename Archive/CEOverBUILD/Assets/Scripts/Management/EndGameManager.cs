using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

// Super sorry if this code is dirty as fuck


public class EndGameManager : MonoBehaviour
{

    //public int timeLeft = 60; //Seconds Overall
    //public Text countdown; //UI Text Object

    public bool startEndGame = false;

    public bool touchGas;

    public GameObject toxicGas;
    public Transform gasTransform;

    public GameObject panel;

    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    public GameObject turretParent;

    void Start()
    {
        panel.SetActive(false);
        turretParent.SetActive(false);
    }

    void Update()
    {
        if (startEndGame)
        {
            Vector3 targetPosition = gasTransform.position;
            toxicGas.transform.position = Vector3.MoveTowards(toxicGas.transform.position, targetPosition, smoothTime * Time.deltaTime);
            if (panel == null)
            {
                SceneManager.LoadScene("YouWin");
            }
        }

        //countdown.text = ("" + timeLeft); //Showing the Score on the Canvas
    }
   

    public void StartEnd()
    {
        //StartCoroutine("LoseTime");
        //Time.timeScale = 1; //Just making sure that the timeScale is right
        startEndGame = true;
        panel.SetActive(true);
        turretParent.SetActive(true);
    }
    
    //Simple Coroutine
    //IEnumerator LoseTime()
    //{
        //while (true)
       // {
          //  yield return new WaitForSeconds(1);
          //  timeLeft--;
       // }
    //}
}
