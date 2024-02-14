using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashLR : MonoBehaviour {

    //How fast the dash is
    public float dashSpeed;

    //How far the dash goes
    public float dashDistance;

    //Is the dash currently available
    public bool dashAvailable;

    //How long before the dash is ready again
    public float dashCooldownTime;

    //Camera references
    public GameObject cameraObject;
    public Camera fpsCamera;

    //End of the dash position
    Vector3 dashEnd;

    //Are we currently dashing
    public bool isDashing = false;


	void Start () {
        fpsCamera = cameraObject.GetComponent<Camera>();
        dashAvailable = true;
    }
	

	void Update () {

        if (Input.GetKeyDown(KeyCode.E))
        {
            //if we're not already dashing and the cooldown is ready
            if(isDashing == false && dashAvailable == true)
            {
                //Setting the dash's beginning point
                Vector3 dashOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit hit;

                if (Physics.Raycast(dashOrigin, fpsCamera.transform.forward, out hit, dashDistance))
                {
                    //If the dash hits something and it's a wall, end the dash early
                    if(hit.transform.tag == "Hookable")
                    {
                        dashEnd = hit.point;
                    }
                }
                else
                {
                    //Dash goes normal distance
                    dashEnd = dashOrigin + fpsCamera.transform.forward.normalized * dashDistance;
                }

                isDashing = true;
                StartCoroutine(DashCooldown());

            }    
            
        }

        //Move the player toward the end point
        if (isDashing == true)
        {
            float distanceToDashEnd = Vector3.Distance(transform.position, dashEnd);
            transform.position = Vector3.Lerp(transform.position, dashEnd, Time.deltaTime * dashSpeed);
            if (distanceToDashEnd < 3)
            {
                isDashing = false;
            }


        }

    }

    private IEnumerator DashCooldown()
    {

        dashAvailable = false;
        yield return new WaitForSeconds(dashCooldownTime) ;
        dashAvailable = true;

    }






}
