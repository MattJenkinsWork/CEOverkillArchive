using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MapValues;

public class PauseMenu : MonoBehaviour {

    GameObject player;
    public vp_FPController playerController;
    public PauseMenuTrigger triggerScript;
    GameObject pauseCanvas;
    GameObject optionsCanvas;

   // [HideInInspector]
    public bool isPaused;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<vp_FPController>();

        GameObject trigger = GameObject.FindGameObjectWithTag("GameManager");
        triggerScript = trigger.GetComponent<PauseMenuTrigger>();

        pauseCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(9).gameObject;
        optionsCanvas = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(10).gameObject;

        Time.timeScale = 0.00001f;
    }

    public void Update()
    {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<PauseMenuTrigger>().isPaused)
        {
            isPaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            isPaused = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Resume()
    {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<PauseMenuTrigger>().playerController.enabled == false)
        {
            playerController.enabled = true;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        triggerScript.isPaused = false;
    }

    public void Options()
    {
        optionsCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        triggerScript.isPaused = true;
    }

    public void MainMenu()
    {
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<PauseMenuTrigger>().playerController.enabled == false)
        {
            playerController.enabled = true;
        }
        Debug.Log("Clicked Main Menu");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        SceneManager.LoadScene("LiftMenu");
        Debug.Log("Zoom");
    }

    public void QuitGame()
    {
        Cursor.visible = false;
        optionsCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        triggerScript.isPaused = false;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        Application.Quit();
    }

}
