using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshManager : MonoBehaviour {

    //Advances the navmesh every time the floor drops. Called from game manager
    //Now that I think about it, this should probably be there

    //List of all navmeshes in the level
    public AirNavMesh[] airNavMeshes;

    [HideInInspector]
    public AirNavMesh currentAirNavMesh;

    int counter = 0;

    // Use this for initialization
    void Start()
    {
        try
        {
            airNavMeshes[0].CreateNavMeshData();
            currentAirNavMesh = airNavMeshes[0];
        }
        catch
        {
            Debug.Log("No airnavmeshes equipped. This isn't a problem for now though");
        }


    }

    public void NextNavMesh()
    {
        counter++;

        try
        {
            airNavMeshes[counter].CreateNavMeshData();
            currentAirNavMesh = airNavMeshes[counter];

        }
        catch
        {
            Debug.Log("No airnavmeshes equipped. This isn't a problem for now though");
        }





    }


}
