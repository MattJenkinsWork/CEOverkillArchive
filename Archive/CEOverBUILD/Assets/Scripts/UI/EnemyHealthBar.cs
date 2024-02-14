using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {


    Enemy attachedEnemy;
    float dividedBar;
    SpriteRenderer rendererer;


    public float sliceWidth = 8;
    public float sliceHeight = 3;

	// Use this for initialization
	void Awake ()
    {
        attachedEnemy = transform.parent.transform.parent.GetComponent<Enemy>();

        rendererer = GetComponent<SpriteRenderer>();
        rendererer.size.Set(sliceWidth, sliceHeight);
        dividedBar = rendererer.size.x / attachedEnemy.maxHealth;
        //Debug.Log(dividedBar);

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (attachedEnemy.currentHealth != attachedEnemy.maxHealth)
        {
            rendererer.size = new Vector2((attachedEnemy.currentHealth * dividedBar), sliceHeight);
        }

        if(attachedEnemy.currentHealth < 1)
        {
            Destroy(transform.parent.gameObject);
        }
        
    }
}
