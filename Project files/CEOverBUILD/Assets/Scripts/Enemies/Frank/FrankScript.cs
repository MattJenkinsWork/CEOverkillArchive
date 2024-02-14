using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrankScript : Enemy {

    //FRANKSCRIPT IS ENTIRELY FOR TESTING AI CAPABILITES, PLEASE DO NOT USE ANYWHERE PROPERLY



	// Use this for initialization
	void Start () {
        //FindPath(true,player.transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        RunStateMachine();

        //FollowPath();
        
    }

    public override void DoDeathEffects()
    {

    }

    public override void ExtraStateHandler(AIState extraAiState)
    {
        
    }

    private void OnDrawGizmos()
    {
        //for (int i = 0; i < openList.Count; i++)
        //{
        //    Gizmos.DrawSphere(openList[i].worldPos, 0.1f); 
        //}
    }

}
