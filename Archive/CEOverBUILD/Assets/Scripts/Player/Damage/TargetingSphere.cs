using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapValues;

public class TargetingSphere : MonoBehaviour {

    SphereCollider sphere;
    vp_FPController fpsCont;
    CharacterController charCont;
    GameManager gManager;
    PlayerManager pManager;

    public float sphereMaxSize;
    public float sphereMinSize;

	// Use this for initialization
	void Start () {
        sphere = GetComponent<SphereCollider>();

        GameObject player = transform.parent.gameObject;
        fpsCont = player.GetComponent<vp_FPController>();
        charCont = player.GetComponent<CharacterController>();
        pManager = player.GetComponent<PlayerManager>();

        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


	}
	
	// Update is called once per frame
	void Update () {

        sphere.radius = MapValuesExtension.Map(fpsCont.MotorAcceleration, gManager.minSpeed, gManager.maxSpeed, sphereMinSize, sphereMaxSize);

        if(charCont.velocity.magnitude > 2)
        {
            Ray ray = new Ray(transform.position, charCont.velocity);

            pManager.enemyTarget = ReverseRaycastPoint(ray);
        }
        else
        {
            pManager.enemyTarget = transform.position;
        }

        

	}

    Vector3 ReverseRaycastPoint(Ray ray)
    {
        RaycastHit hit;

        ray.origin = ray.GetPoint(sphere.radius * 3);
        ray.direction = -ray.direction;

        string[] layerNames = new string[1];
        layerNames[0] = "PredictionSphere";


        if(Physics.Raycast(ray.origin,ray.direction, out hit, sphere.radius * 3, LayerMask.GetMask(layerNames), QueryTriggerInteraction.Ignore))
        {
            return hit.point;
        }
        else
        {
            
            //Debug.LogError("This should never occur. Something is wrong with the targeting sphere. Call Matt");
            return Vector3.zero;
        }

    }
 
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawSphere(pManager.enemyTarget, 0.2f);
    //}



}
