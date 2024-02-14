using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuTrigger : MonoBehaviour
{
    public GameObject player;
    public vp_FPController playerController;
    public GameObject pauseCanvas;
    public GameObject optionsCanvas;

 //  [HideInInspector]
    public bool isPaused;

    KeyCode key = 0;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<vp_FPController>();

        pauseCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(9).gameObject;
        optionsCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(10).gameObject;
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(false);

        isPaused = false;


        if (Application.isEditor && !optionsCanvas.activeInHierarchy)
        {
            key = KeyCode.N;
        }
        else
        {
            key = KeyCode.Escape;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.GetComponent<PlayerManager>().dead == false) && (Input.GetKeyDown(key)))
        {
            if (isPaused)
            {
                Time.timeScale = 1f;
                Resume();
            }
            else
            {
                Time.timeScale = 0.00001f;
                Pause();
            }
        }
    }

    public void Pause()
    {
        playerController.enabled = false;
        pauseCanvas.SetActive(true);
        isPaused = true;
    }

    public void Resume()
    {
        playerController.enabled = true;
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        isPaused = false;
    }
}

