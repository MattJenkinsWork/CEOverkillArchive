using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfTutorial : MonoBehaviour {



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("LiftMenu");
        }
    }
}
