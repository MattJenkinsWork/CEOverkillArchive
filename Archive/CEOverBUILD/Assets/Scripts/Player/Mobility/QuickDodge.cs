using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickDodge : MonoBehaviour {

    vp_FPController controller;
    private Dictionary<KeyCode, float> timeLastTapped = new Dictionary<KeyCode, float>();
    public float doubleTapInterval = 0.5f;
    public float dodgeForce = 10f;

    // Use this for initialization
    void Start () {
		controller = GetComponent<vp_FPController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (DidDoubleTap(KeyCode.A))
        {
            controller.AddForce(transform.right * -1 * dodgeForce * Time.deltaTime);
        }
        if (DidDoubleTap(KeyCode.D))
        {
            controller.AddForce(transform.right * dodgeForce * Time.deltaTime);
        }
        if (DidDoubleTap(KeyCode.W))
        {
            controller.AddForce(transform.forward * dodgeForce * Time.deltaTime);
        }
        if (DidDoubleTap(KeyCode.S))
        {
            controller.AddForce(transform.forward * -1  * dodgeForce * Time.deltaTime);
        }
    }

    public bool DidDoubleTap(KeyCode k)
    {
        if (!timeLastTapped.ContainsKey(k)) timeLastTapped.Add(k, -9999f);
        bool pressed = Input.GetKeyDown(k);
        float previousTap = timeLastTapped[k];
        if (pressed) timeLastTapped[k] = Time.time;
        return (pressed && Time.time < previousTap + doubleTapInterval);
    }

}
