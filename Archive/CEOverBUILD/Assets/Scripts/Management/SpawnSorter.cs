using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSorter : MonoBehaviour {
    //Sorts through all spawnpoints and returns a list of vector3s used for spawning enemies

    //All of the objects tagged "spawnpoint"
    GameObject[] spawnObjects = new GameObject[1000];

    //All  of the transforms for objects tagged spawnpoint
    Transform[] spawnTransforms = new Transform[1000];

    //The spawnpoint positions that's passed to the game manager
    public List<Vector3> floorSpawns = new List<Vector3>();

    //Spawn points where the spawn is designated
    public List<SpawnDesignator> floorDesignated = new List<SpawnDesignator>();

    //Gather all spawnpoints
    public void GetSpawns() {
        spawnObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for (int i = 0; i < spawnObjects.Length; i++)
        {
            if (spawnObjects[i] == null)
                break;

            spawnTransforms[i] = spawnObjects[i].GetComponent<Transform>();
        }

        //CreateSpawnList(0);
	}
	

	public List<Vector3> CreateSpawnList(int floorNo)
    {
        floorSpawns.Clear();
        floorDesignated.Clear();

        string[] splitNums = new string[2];
        const char splitter = '.';

        //Scans all spawnpoints for their name. If the name is formatted correctly and the number matches the current floor, it is added spawn list
        for (int i = 0; i < spawnTransforms.Length; i++)
        {
            if (spawnTransforms[i] == null)
                break;

            if (!spawnTransforms[i].gameObject.name.Contains("."))
            {
                Debug.LogError("Spawn name formatted incorrectly :[ Correct format is 'X.X' as 'floor.induvidualId' Quitting...");
                Debug.LogError(spawnTransforms[i].gameObject.name);
                break;
            }

            splitNums = spawnTransforms[i].gameObject.name.Split(splitter);

            //Check the first part of the name for the floor number
            if (int.Parse(splitNums[0]) == floorNo)
            {
                //Add it if it's legitimate
                floorSpawns.Add(spawnTransforms[i].position);

                //If it has a spawn designator, add the enemy to the list of designated enemies, else add nothing
                if (spawnTransforms[i].gameObject.GetComponent<SpawnDesignator>())
                {
                    floorDesignated.Add(spawnTransforms[i].gameObject.GetComponent<SpawnDesignator>());
                }
                else
                {
                    floorDesignated.Add(null);
                }

            }
        
        }

        if (!(floorSpawns.Count > 0))
        {
            Debug.LogWarning("No spawns found for floor " + floorNo);
        }

        return floorSpawns;

       
      

    }
}
