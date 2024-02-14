using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDestroy : MonoBehaviour {

    public int maxHealth;
    public int health;


    private void Awake()
    {
        health = maxHealth;
    }

    //Called by SwordDamage
    public void PropTakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
