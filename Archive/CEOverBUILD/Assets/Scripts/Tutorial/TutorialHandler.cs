using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour {


    public PlayerManager pManager;

    public GameObject testDummy1;
    public GameObject testDummy2;

    public GameObject combatDummy1;
    public GameObject combatDummy2;
    public GameObject combatDummy3;
    public GameObject combatDummy4;
    public GameObject combatDummy5;


    public GameObject tutorialOverlay;
    //public GameObject checkpoint1;
    //public GameObject checkpoint2;

    //public GameObject rangedDummy1;

    public GameObject combatBarrier1;
    public GameObject combatBarrier2;
    //public GameObject combatBarrier3;

    // Use this for initialization
    void Start () {
        pManager.tutorial = true;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(testDummy1 == null)
        {
            combatBarrier1.SetActive(false);
           // checkpoint1.SetActive(true);

        }

        if (testDummy2 == null)
        {
            combatBarrier2.SetActive(false);
            //checkpoint2.SetActive(true);
        }

        if (tutorialOverlay.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                tutorialOverlay.SetActive(false);
            }
        }

        if (combatDummy1 == null && combatDummy2 == null && combatDummy3 == null && combatDummy4 == null && combatDummy5 == null)
        {
            SceneManager.LoadScene("LiftMenu");

        }

        //if (combatDummy1 == null && combatDummy2 == null && combatDummy3 == null && combatDummy4 == null && combatDummy5 == null)
        //{
        //    combatBarrier3.SetActive(false);
        //}
    }

}
