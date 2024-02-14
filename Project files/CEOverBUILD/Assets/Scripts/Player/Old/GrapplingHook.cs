using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour {

    public GameObject hook;
    public GameObject hookHolder;
    public GameObject hookedObj;

    public float hookTravelSpeed;
    public float playerTravelSpeed;

    public static bool fired;
    public bool hooked;

    public float maxDistance;
    private float currentDistance;

    Collider hookCollider;

    private void Start()
    {

        hookCollider = hook.GetComponent<Collider>();

    }

    void Update()
    {
        //firing the hook
        if (Input.GetMouseButtonDown(1) && fired == false)
        {
            hookCollider.enabled = true;
            fired = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
            ReturnHook();

        if (fired == true && hooked == false)
        {
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed);
            currentDistance = Vector3.Distance(transform.position, hook.transform.position);

            if (currentDistance >= maxDistance)
                ReturnHook();
        }

        if(hooked == true)
        {
            hook.transform.parent = hookedObj.transform;
            transform.position = Vector3.Lerp(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);
            this.GetComponent<vp_FPController>().PhysicsGravityModifier = 0.0f;
            this.GetComponent<vp_FPController>().MotorFreeFly = true;

            if (distanceToHook < 3)
                ReturnHook();
        } else
        {
            if (GetComponent<DashLR>().isDashing == false && GetComponent<WallRunning>().IsWallrunning == false)

                this.GetComponent<vp_FPController>().PhysicsGravityModifier = 0.4f;

            hook.transform.parent = hookHolder.transform;
            this.GetComponent<vp_FPController>().MotorFreeFly = false;
        }

        if (hooked == false && fired == false)
            hookCollider.enabled = false;
    }

    void ReturnHook()
    {
        hook.transform.rotation = hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;
    }


}
