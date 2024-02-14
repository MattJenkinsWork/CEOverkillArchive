using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : Projectile {

    public float range;

    private void Start()
    {
        transform.localScale = new Vector3(1,1,range);

        for (int i = 0; i < range / 2; i++)
        {
            transform.position += transform.forward;
        }

        
    }

    private void Update()
    {
        LifetimeTick();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public override void Destructed()
    {
        Destroy(gameObject);
    }
}
