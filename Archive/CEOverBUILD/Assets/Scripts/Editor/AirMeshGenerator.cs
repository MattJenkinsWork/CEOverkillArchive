using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MakeAirNavmesh))]
[RequireComponent(typeof(NavMeshGizmoDrawer))]
public class AirMeshGenerator : Editor {

    //The positions of each node, stored as a vector3
    public List<Vector3> nodePositions;

    //Size of the airnavmesh
    int xSize = 20;
    int ySize = 3;
    int zSize = 20;

    //The position from which the mesh generates
    Vector3 startPos;

    //The spacing between nodes on the xz and y axis
    float xzSpace = 5;
    float ySpace = 5;

    //Vector3s used to dictate how far the placer should move when it moves in a direction. These are defined by zxSpace and ySpace
    private Vector3 xMove;
    private Vector3 yMove;
    private Vector3 zMove;

    //The distance apart that nodes must be below in order to link
    public float distanceTolerance;

    public override void OnInspectorGUI()
    {
        
        GUILayout.Label("~~~~ Navmesh Maker Settings ~~~~");

        //Creating fields
        GUILayout.Label("Mesh Generation");
        xSize = EditorGUILayout.IntField("Max X Size", xSize);
        ySize = EditorGUILayout.IntField("Max Y Size", ySize);
        zSize = EditorGUILayout.IntField("Max Z Size", zSize);

        startPos = EditorGUILayout.Vector3Field("Start Position", startPos);

        xzSpace = EditorGUILayout.FloatField("Node spacing X and Z", xzSpace);
        ySpace = EditorGUILayout.FloatField("Node spacing Y", ySpace);


        GUILayout.Label("Node Linking");
        distanceTolerance = EditorGUILayout.FloatField("Max link distance", distanceTolerance);



        if (GUILayout.Button("Generate Mesh"))
            GenerateWorldVerts();

        if(AssetDatabase.LoadAssetAtPath("Assets/Scripts/Tools/AirNavMeshes/FinishedMeshes/SavedMesh.asset", typeof(AirNavMesh)))
        {
            EditorGUILayout.HelpBox("A saved mesh already exists, saving will replace this mesh!", MessageType.Warning);
        }
            
        if (GUILayout.Button("Save current mesh"))
            SaveMesh();

    }



    void GenerateWorldVerts()
    {

        //Setting up possible moves
        xMove = new Vector3(xzSpace, 0, 0);
        yMove = new Vector3(0, ySpace, 0);
        zMove = new Vector3(0, 0, xzSpace);

        //Resetting worldVerts
        nodePositions = new List<Vector3>();

        //Setting current and row positions for the placer
        Vector3 currentPos = startPos;
        Vector3 rowStartPos = startPos;

        //Current amount of x, y and z nodes placed
        int currentX = 0;
        int currentY = 0;
        int currentZ = 0;

        for (int i = 0; i < (xSize) * (ySize) * (zSize); i++)
        {
            

            //if the xSize has been reached, move on z and reset the x counter
            if (currentX == xSize && currentZ != zSize)
            {
                rowStartPos = rowStartPos + zMove;
                currentPos = rowStartPos;
                currentX = 0;
                currentZ++;
            }

            //If the zSize has been reached, move upwards and reset both x and z counters
            if (currentY < ySize && currentZ == zSize)
            {
                rowStartPos = startPos + (yMove * (currentY + 1));
                currentPos = rowStartPos;
                currentX = 0;
                currentZ = 0;
                currentY++;
            }

            //If there aren't enough x nodes, create one
            if (currentX < xSize)
            {

                //If we cannot detect anything in front of the current point and extracheck returns false, we can place a node, move the placer forward and increment on x
                if (!Physics.Raycast(currentPos - xMove + (xMove * 0.1f), xMove, xzSpace) && !ExtraCheck(currentPos - xMove * 0.1f))
                {
                    nodePositions.Add(currentPos);
                    currentPos += xMove;
                    currentX++;
                }
                else
                {
                    //If we see object very nearby, move along and skip this
                    currentPos += xMove;
                    currentX++;
                }


            }

        }

        //Create an instance of an airnavmesh and then fill it with our placed node positions
        AirNavMesh currentMesh = CreateInstance<AirNavMesh>();
        currentMesh.worldPos = nodePositions;

        //Save it under current meshes
        AssetDatabase.CreateAsset(currentMesh, "Assets/Scripts/Tools/AirNavMeshes/currentMesh.asset");

    }

    //Checks for surrounding colliders. Called before moving the placer
    bool ExtraCheck(Vector3 currentPos)
    {

        if (ReverseRaycast(currentPos,zMove,xzSpace * 1))
        {
            return true;
        }

        if (ReverseRaycast(currentPos,-zMove,xzSpace * 1))
        {
            return true;
        }

        if (ReverseRaycast(currentPos,yMove,xzSpace * 1))
        {
            return true;
        }

        if (ReverseRaycast(currentPos, -yMove, xzSpace * 1))
        {
            return true;
        }

        return false;
    }

    //Casts a ray, goes a distance along it and then reverses. This checks if we're inside any objects
    bool ReverseRaycast(Vector3 origin, Vector3 direction, float distanceBeforeTurn)
    {
        Ray ray = new Ray(origin,direction);

        ray.origin = ray.GetPoint(distanceBeforeTurn);
        ray.direction = -ray.direction;

        if (Physics.Raycast(ray, distanceBeforeTurn))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //Saves the existing current mesh into a different folder and renames it
    //Also adds the distance tolerance for the mesh to create itself based on
    void SaveMesh()
    {
        AirNavMesh mesh;
        mesh = (AirNavMesh)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Tools/AirNavMeshes/currentMesh.asset", typeof(AirNavMesh));

        mesh.name = "savedMesh";
        mesh.connectionRange = distanceTolerance; 
        mesh.CreateNavMeshData();

        AssetDatabase.CopyAsset("Assets/Scripts/Tools/AirNavMeshes/currentMesh.asset", "Assets/Scripts/Tools/AirNavMeshes/FinishedMeshes/savedMesh.asset");

        Debug.Log("Saved mesh!");

    }
   
}

