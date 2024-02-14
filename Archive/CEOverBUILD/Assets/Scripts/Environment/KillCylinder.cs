using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCylinder : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().EnemyDead(Enemy.DeathType.explosion);
        }
    }
}
