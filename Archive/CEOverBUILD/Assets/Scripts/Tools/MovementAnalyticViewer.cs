using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[ExecuteInEditMode]
public class MovementAnalyticViewer : MonoBehaviour {

    public TextAsset[] logs;
    public bool view;

    List<GameObject> currentLogObjs = new List<GameObject>();


    List<Vector3> displayedPoints = new List<Vector3>();

    

    void RefreshDisplay()
    {
        if (view)
        {
            foreach (GameObject point in currentLogObjs)
            {
                DestroyImmediate(point);
            }


            currentLogObjs.Clear();

            for (int i = 0; i < logs.Length; i++)
            {
                string contents = logs[i].ToString().Trim();

                //Debug.Log(contents);

                string[] rawVector = contents.Split('M');

                //Debug.Log(rawVector[3]);

                List<Vector3> vector3s = new List<Vector3>();

                for (int p = 0; p < rawVector.Length; p++)
                {
                    string[] numbers = rawVector[p].Split(',');

                    //if(numbers[0].)

                    //Debug.Log(numbers[0]);

                    try
                    {
                        vector3s.Add(new Vector3(float.Parse(numbers[0]), float.Parse(numbers[1]), float.Parse(numbers[2])));
                    }
                    catch
                    {
                        //This try catch is super super filthy but hey
                    }
                   

                    //Debug.Log(numbers[2]);
                }

                for (int o = 0; o < vector3s.Count; o++)
                {
                    displayedPoints.Add(vector3s[o]);
                }


            }


            if (displayedPoints == null)
                return;

            GameObject parent = new GameObject("LogParent");

            currentLogObjs.Add(parent);

            for (int i = 0; i < displayedPoints.Count; i++)
            {
                GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                point.name = ("point" + i.ToString());
                point.transform.position = displayedPoints[i];

                point.transform.SetParent(parent.transform);

                currentLogObjs.Add(point);

            }




        }

        displayedPoints.Clear();

        view = false;

    }

    private void Update()
    {
        RefreshDisplay();
    }

}
