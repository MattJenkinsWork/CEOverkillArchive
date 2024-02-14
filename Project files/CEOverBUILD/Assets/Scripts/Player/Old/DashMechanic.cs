using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashMechanic : MonoBehaviour
{
    public float Force = 10f;
    public float ForceTime = 1f;
    private float AvailableTime;
    private vp_FPController m_Controller;
    public bool isDashing;
    bool freeze;
    CursorLockMode cursorNotLock;
    CursorLockMode cursorLock;
    public Text debugText;
    public GameObject dashCollider;
    public Transform hitboxSpawner;
    GameObject dashColliderPrefab;

    void Awake()
    {
        cursorNotLock = CursorLockMode.None;
        cursorLock = CursorLockMode.Locked;
        m_Controller = GameObject.FindObjectOfType(typeof(vp_FPController)) as vp_FPController;
        AvailableTime = ForceTime;
    }

    void Update()
    {
        //Debug.Log(m_Controller.Velocity.magnitude + "      " + AvailableTime);
        bool store = true ;
        Vector3 storedVelocity = new Vector3 (0,0,0);

        if (Input.GetKeyDown(KeyCode.E) && AvailableTime > 0)
        {
            if (store)
            {
                storedVelocity = m_Controller.Velocity;
                store = false;
            }

            m_Controller.PhysicsGravityModifier = 0.0f;

            AvailableTime -= Time.deltaTime;

            m_Controller.AddForce(transform.forward * Force * 0.001f);

            dashColliderPrefab = Instantiate(dashCollider, hitboxSpawner.position, Quaternion.identity);
            dashColliderPrefab.transform.parent = gameObject.transform;

            isDashing = true;

            Cursor.lockState = cursorNotLock;
            
        }

        if (AvailableTime <= 0)
        {
            m_Controller.PhysicsGravityModifier = 0.4f;
            isDashing = false;
        }

        

        if (Input.GetKeyUp(KeyCode.E))
        {
            AvailableTime = ForceTime;
            m_Controller.Velocity.Set(storedVelocity.x, storedVelocity.y, storedVelocity.z);
            store = true;
            Cursor.lockState = cursorLock;
            isDashing = false;
            Destroy(dashColliderPrefab);
            
        }
        

        if (isDashing)
        {
            debugText.text = "Yes";
            m_Controller.Velocity.Set(m_Controller.Velocity.x, 0, m_Controller.Velocity.z);
        }
        else
        {
            debugText.text = "No";
        }

    }
}
