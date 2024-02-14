using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MakeCircle))]
public class MakeCircleEditor : Editor {

    //Circle parameters
    float radius;
    int noOfSegments;
    Vector3 rotationOffset;
    Object segmentPrefab;
    Vector3 centre;


    //List of current segments
    List<Object> segments = new List<Object>();

    //Used in save system (Does not work)
    float radiusSave;
    int noOfSegmentsSave;
    public Object segmentPrefabSave;



    
    public override void OnInspectorGUI()
    {

        GUILayout.Label("~~~~ Circle Maker Settings ~~~~");

        //Creating fields
        noOfSegments = EditorGUILayout.IntField("Number of Segments",noOfSegments);
        radius = EditorGUILayout.FloatField("Radius",radius);
        rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset",rotationOffset);
        centre = EditorGUILayout.Vector3Field("Centre",centre);
        segmentPrefab = EditorGUILayout.ObjectField("Segment Prefab",segmentPrefab, typeof(GameObject), false); //GameObject,segmentPrefab)

        //Button functions
        if (GUILayout.Button("Build Circle"))
            BuildCircle();

        if (GUILayout.Button("Remove Last Circle"))
            RemoveCircle();
    }


    void BuildCircle()
    {
        //Clears existing segments list
        segments.Clear();

        //finds diameter and how many degrees should be assigned per segment
        float diameter = 2 * 3.14f * radius;
        float degreesPerSegment = 360 / (float)noOfSegments;
        Debug.Log(degreesPerSegment);
        Vector3 pos = new Vector3();

        Debug.Log("Created circle of diameter " + diameter + " and degrees per segment of " + degreesPerSegment);

        
        float currentDegrees = 0;

        //Creates each segment
        for (int i = 0; i < noOfSegments; i++)
        {
            
            currentDegrees = degreesPerSegment * i;

            Debug.Log(currentDegrees);

            pos.x = centre.x + radius * Mathf.Sin(currentDegrees * Mathf.Deg2Rad);
            pos.y = centre.y;
            pos.z = centre.z + radius * Mathf.Cos(currentDegrees * Mathf.Deg2Rad);

            Quaternion rot = Quaternion.FromToRotation(Vector3.forward + rotationOffset, centre - pos );

            segments.Add(Instantiate(segmentPrefab, pos, rot));

        }
        

    }

    void RemoveCircle()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            DestroyImmediate(segments[i]);
        }
    }
}
