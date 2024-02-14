using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public TextMesh floorText;
    private int floorCount = 0;
    public float floorTime = 1;

    IEnumerator UpdateFloor()
    {
        floorCount++;

        if (floorCount < 10)
            floorText.text = "0" + floorCount.ToString();
        else
            floorText.text = floorCount.ToString();

        yield return new WaitForSeconds(floorTime);

        if (floorCount > 98)
            floorCount = 0;

        StartCoroutine(UpdateFloor());
    }

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        if (floorText != null)
            StartCoroutine(UpdateFloor());

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        if (Cursor.lockState != CursorLockMode.Confined)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PlayGame()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Test2");
    }

    public void SampleScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("SampleScene");
    }

    public void EnemyRoom()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("EnemyTestRoom");
    }

    public void GenerationRoom()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Matt's Generation Test Room");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
