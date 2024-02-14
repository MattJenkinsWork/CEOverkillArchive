using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    public GameObject player;
    public float maxLeftDistance;
    public float maxRightDistance;
    vp_FPController controller;
    public bool IsWallrunning = false;
    public float wallRunSpeed;
    bool fallReset = true;

    public float targetCamRot = 0;

    RaycastHit hitRight;
    RaycastHit hitLeft;
    public bool jumpReset = false;

    public float fallHeight;

    public Vector3 normal;

    void Start()
    {
        controller = GetComponent<vp_FPController>();
        
    }

    void Update()
    {
        targetCamRot = 0;
        fallHeight = controller.m_FallSpeed;
        if (IsWallrunning == false)
        {
            fallReset = true;

        }


        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.left), out hitLeft, maxLeftDistance))
        {
            if(hitLeft.transform.tag == "Hookable" || hitLeft.transform.tag == "ClearAfterFall")
            {
                if (!controller.Grounded)
                {
                    targetCamRot = -15;
                    //Vector3 wallDirection = Vector3.Cross(hitLeft.normal, Vector3.up);
                    WallRunLeft();
                }
            }
            
        }
        else if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.right), out hitRight, maxRightDistance))
        {
            if (hitRight.transform.tag == "Hookable" || hitRight.transform.tag == "ClearAfterFall")
            {
                if (!controller.Grounded)
                {
                    targetCamRot = 15;
                    //Vector3 wallDirection = Vector3.Cross((hitRight.point - transform.position).normalized, Vector3.up);
                    WallRunRight();
                }
            }
        }
        else
        {
            IsWallrunning = false;
        }
    }

    void WallRunLeft()
    {
        if (fallReset)
        {
            //Debug.Log(fallReset);
            controller.m_FallSpeed = 0.0f;
            fallReset = false;
        }

        if(jumpReset == false)
        {
           // Debug.Log("Jump Reset True");
            jumpReset = true;
        }
        Vector3 wallDirection = Vector3.Cross(hitLeft.normal, Vector3.up);
        normal = hitLeft.normal;
        controller.AddForce(wallDirection * wallRunSpeed * Time.deltaTime);
        IsWallrunning = true;
    }

    void WallRunRight()
    {
        if (fallReset)
        {
            //Debug.Log(fallReset);
            controller.m_FallSpeed = 0.0f;
            fallReset = false;
        }

        if (jumpReset == false)
        {
           // Debug.Log("Jump Reset True");
            controller.m_FallSpeed = 0f;
            jumpReset = true;
        }
        normal = hitRight.normal;
        Vector3 wallDirection = Vector3.Cross((hitRight.point - transform.position).normalized, Vector3.up);
        controller.AddForce(wallDirection * wallRunSpeed * Time.deltaTime);
        IsWallrunning = true;
    }
}
