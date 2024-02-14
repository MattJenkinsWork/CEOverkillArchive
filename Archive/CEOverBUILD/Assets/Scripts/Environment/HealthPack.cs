using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    public int amountHealed;

    public void Start()
    {
        amountHealed = 10;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.GetComponent<PlayerManager>().currentHealth == col.GetComponent<PlayerManager>().maxHealth)
            {
                Debug.Log("Cannot heal! Max Health!");
            }
            else
            {
                Debug.Log("Add Health");
                col.GetComponent<PlayerManager>().PlayerRecoverDamage(amountHealed);
                Destroy(this.gameObject);
            }
       
        }
    }
}
