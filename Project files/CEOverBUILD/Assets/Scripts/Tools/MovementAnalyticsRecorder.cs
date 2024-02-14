using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnalyticsRecorder : MonoBehaviour {

    public List<Vector3> path = new List<Vector3>();

    int counter = 0;

    public int counterMax = 100;

    int attemptNo;

    string writeText;

    string filePath;

    public int maximumAmountOfLogs = 100;

    bool fileCanBeCreated = false;

    private void Start()
    {
        path.Add(Vector3.zero);

        if(!Directory.Exists(Application.dataPath + "/Logs"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Logs");
        }

        filePath = Application.dataPath + "/Logs/log" + attemptNo.ToString() + ".txt";

        for (int i = 0; i < maximumAmountOfLogs; i++)
        {
            if (File.Exists(filePath))
            {
                attemptNo++;
                filePath = Application.dataPath + "/Logs/log" + attemptNo.ToString() + ".txt";
            }
            else
            {
                fileCanBeCreated = true;
                break;
            }
        }

    }



    // Update is called once per frame
    void Update () {

        counter++;

        if(counter > counterMax)
        {
            path.Add(transform.position);
            counter = 0;
        }

	}

    private void OnGameEnd()
    {
        if (fileCanBeCreated)
        {
            string log = "";

            foreach (Vector3 point in path)
            {
                log += point.x + "," + point.y + "," + point.z + "M"; //point.ToString() + "-";
            }

            File.WriteAllText(filePath, log);
        }
    }

    private void OnDestroy()
    {
        OnGameEnd();
    }

}
