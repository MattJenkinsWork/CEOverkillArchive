using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDoorOpen : MonoBehaviour {

    public int timeBeforeAnimationStart;
    Animator m_Animator;

    // Use this for initialization
    void Start () {

        m_Animator = gameObject.GetComponent<Animator>();
        Invoke("AnimationStart", timeBeforeAnimationStart);

	}
	
	// Update is called once per frame
	void Update () {
		

	}

    void AnimationStart ()
    {
        m_Animator.SetBool("StartAnimationCycle", true);

    }

    void DestroyBlackDoor()
    {
        m_Animator.SetBool("OpenDoor", true);
    }
}
