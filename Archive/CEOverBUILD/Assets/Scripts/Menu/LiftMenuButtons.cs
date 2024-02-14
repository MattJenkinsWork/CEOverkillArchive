using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LiftMenuButtons : MonoBehaviour
{

    public Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name == "PlayGame" && Input.GetMouseButtonDown(0))
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("TowerBuild");
            }
            if (hit.collider.gameObject.name == "Tutorial" && Input.GetMouseButtonDown(0))
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("Tutorial2");
            }
            if (hit.collider.gameObject.name == "Quit" && Input.GetMouseButtonDown(0))
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                Application.Quit();
            }
        }
    }
}
