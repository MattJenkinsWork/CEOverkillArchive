using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(MakeAirNavmesh))]
public class NavMeshGizmoDrawer : MonoBehaviour {

    //Variables to allow node showing
    public bool showNodes = false;
    public bool showSavedMesh = false;

    public AirNavMesh meshToShow;

    AirNavMesh airNav;

    //Copy of the navmesh's nodes
    List<List<Node>> nodesList = new List<List<Node>>();

    private void Awake()
    {
        airNav= ScriptableObject.CreateInstance<AirNavMesh>();
    }



    private void Update()
    {

        if(meshToShow == null)
        {
            airNav = null;

            //Loads the navmesh you selected, or the saved one
            if (showNodes)
            {
                airNav = (AirNavMesh)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Tools/AirNavMeshes/currentMesh.asset", typeof(AirNavMesh));
            }

            if (showSavedMesh)
            {
                airNav = (AirNavMesh)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Tools/AirNavMeshes/FinishedMeshes/savedMesh.asset", typeof(AirNavMesh));
            }
        }
        else
        {
            airNav = meshToShow;
        }

        

    }

    
    //Draws the lines and dots where the navmesh specifies
    private void OnDrawGizmos()
    {

        if (showNodes)
        {
            if (airNav.worldPos == null)
                return;
            Gizmos.color = Color.black;
            for (int i = 0; i < airNav.worldPos.Count; i++)
            {
                Gizmos.DrawSphere(airNav.worldPos[i], 0.1f);
            }
        }

        if (showSavedMesh)
        {
            airNav.CreateConnectedNodes();
            nodesList = airNav.connectedNodes;


            for (int i = 0; i < airNav.nodes.Count; i++)
            {
                for (int m = 0; m < nodesList[i].Count; m++)
                {
                    Debug.DrawLine(airNav.nodes[i].worldPos, nodesList[i][m].worldPos,Color.black);
                }
            }

        }
    }
}
