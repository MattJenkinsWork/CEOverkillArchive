using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIndicator : MonoBehaviour {

    public Vector3 target;
    public Transform arrow;
    public Vector3 pointOfArrow;

    void Update ()
    {
        arrows();
        //Debug.Log(target);
	}

    void arrows ()
    {
        Vector3 direction = Camera.main.WorldToScreenPoint(target);
        pointOfArrow.z = Mathf.Atan2((arrow.transform.position.y - direction.y), (arrow.transform.position.x - direction.x)) * Mathf.Rad2Deg - 270;
        arrow.transform.rotation = Quaternion.Euler(pointOfArrow);
    }
}