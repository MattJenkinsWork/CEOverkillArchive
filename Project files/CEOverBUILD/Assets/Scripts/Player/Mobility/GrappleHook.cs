using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour {

    public GameObject hook;
    public GameObject hookHolder;

    public GameObject cameraObject;
    public Camera fpsCamera;

    public float hookTravelSpeed;
    public float playerTravelSpeed;
    public float maxDistance;
    private float currentDistance;
    public int hookDistanceFromObject;

    
    public bool fired;
    public bool hooked;
    public bool didItHit;

    Vector3 grappleEnd;

    void Start()
    {
        fpsCamera = cameraObject.GetComponent<Camera>();

    }

	void Update () {

        if (Input.GetMouseButtonDown(1) && fired == false)
        {
            Vector3 grappleOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
            if (Physics.Raycast(grappleOrigin, fpsCamera.transform.forward, out hit, maxDistance))
            {
                grappleEnd = hit.point;
                didItHit = true;
            }
            else
            {
                grappleEnd = grappleOrigin + fpsCamera.transform.forward.normalized * maxDistance;
                didItHit = false;
            }
            hook.transform.parent = null;
            fired = true;
        }

        if (fired == true && hooked == false)
        {

            hook.transform.position = Vector3.MoveTowards(hook.transform.position, grappleEnd, Time.deltaTime * hookTravelSpeed);

            currentDistance = Vector3.Distance(hook.transform.position, grappleEnd);

            if (currentDistance < hookDistanceFromObject)
            {

                if (didItHit == true)
                {
                    hooked = true;

                }

                if (didItHit == false)
                {
                    ReturnHook();

                }
            }

            
        }

        if(hooked == true)
        {
            transform.position = Vector3.Lerp(transform.position, grappleEnd, Time.deltaTime * playerTravelSpeed);
            float distanceToHook = Vector3.Distance(transform.position, grappleEnd);
            if (distanceToHook < 4)
                ReturnHook();

            if (Input.GetMouseButtonDown(1))
                ReturnHook();
        }

        if (Input.GetKeyDown(KeyCode.R))
            ReturnHook();

    }

    void ReturnHook()
    {
        hook.transform.parent = hookHolder.transform;
        hook.transform.rotation = hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;
    }
}
