using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCam : MonoBehaviour {

    public GameObject canvas;
    public GameObject flyCanvas;
    public float cameraSensitivity;
    public float climbSpeed;
    public float normalMoveSpeed;
    public float slowMoveSpeed;
    public float fastMoveSpeed;
    public float rotationX;
    public float rotationY;
    public bool enableMouseControls;

	// Use this for initialization
	void Start ()
    {
        // values
        cameraSensitivity = 50;
        climbSpeed = 4;
        normalMoveSpeed = 10;
        slowMoveSpeed = 1;
        fastMoveSpeed = 3;
        rotationX = 0;
        rotationY = 0;

        canvas = GameObject.FindGameObjectWithTag("Canvas");
        flyCanvas.SetActive(true);
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        enableMouseControls = false;
	}
	
	void Update ()
    {
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationX += cameraSensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotationX -= cameraSensitivity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rotationY += cameraSensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rotationY -= cameraSensitivity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
        {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveSpeed) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * fastMoveSpeed) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveSpeed) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * slowMoveSpeed) * Input.GetAxis("Horizontal") * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += transform.up * climbSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position -= transform.up * climbSpeed * Time.deltaTime;
        }
       
        if (Input.GetKey(KeyCode.I))
        {
            flyCanvas.SetActive(false);
        }

        if (Input.GetKey(KeyCode.N))
        {
            enableMouseControls = true;
        }

        if (Input.GetKey(KeyCode.M))
        {
            enableMouseControls = false;
        }

        if (enableMouseControls == true)
        {
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            Cursor.lockState = CursorLockMode.None;
            canvas.SetActive(true);
        }
    }
}
