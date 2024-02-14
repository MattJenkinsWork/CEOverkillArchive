using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateFloor : MonoBehaviour {

    //THIS SCRIPT IS ENTIRELY DEFUNCT!
    //ALL LEVEL GENERATION IS NOW HANDLED IN GAMEMANAGER
    


    //Goodnight sweet prince


 /*   
    [HideInInspector]
    GameManager manager;

    [HideInInspector]
    TowerDataHolder data;

    //How many floors each tower has
    const int tower1MaxFloors = 10;

    //How many different types of floor there are
    const int tower1MaxVariants = 3;

    //Boss level info for each tower
    FloorData tower1Final;

    //Enables randomisation for level variants
    public bool enableRandom;

    //Does the stage spawn when the scene is started?
    public bool spawnStageOnStart = false;

    //Where is the bottom of the current floor?
    public float bottomOfCurrentStage;

    int chosenFloor = 0;

    private void Awake()
    {
        manager = GetComponent<GameManager>();
        data = GetComponent<TowerDataHolder>();
    }

    // Use this for initialization
    void Start () {

        if (spawnStageOnStart)
        {
            manager.FloorController();
        }
            
    }
	

    //If this causes problems we may have to swap to an enable system instead
    //Reads a file called fd_X_Y where X is the current towerID and Y is the current floor
    //To create a new floor, create a new FloorData and place it within the TowerDataHolder on the GameManager
    //Ideally you should talk to Matt about this if you don't know what it does
    public void CreateNewFloor(GameManager.TowerType currentTower, bool isFinalFloor)
    {
        
        float heightAdjustment = 0;

        //Create a new floordata to fill with info
        ScriptableObject floorDataObject = ScriptableObject.CreateInstance("FloorData");

        FloorData floorData = (FloorData)floorDataObject;

        //Monitors whether the relevant data was found or not
        bool dataFound = false;

        
        int towerID = (int)currentTower;

        //Choose a random variant
        //Random.Range(0, tower1MaxVariants);

        if (!enableRandom)
            chosenFloor = 0;

        //Finds information about the floor using Chosen floor
        switch (towerID)
        {
            case 1:
                floorData = data.tower1Data[chosenFloor];
                if (isFinalFloor) floorData = data.finalFloor[towerID - 1];
                break;
            /*case 2:
                floorData = data.tower2Data[chosenFloor];
                if (isFinalFloor) floorData = data.finalFloor[towerID - 1];
                break;
            case 3:
                floorData = data.tower3Data[chosenFloor];
                if (isFinalFloor) floorData = data.finalFloor[towerID - 1];
                break;
                
        }


        if (floorData != null)
        {
            dataFound = true;
        }

       

        //Take data if the asset is found and transfer it to the manager. Sound alarm if not
        if (!dataFound)
        {
            Debug.Log("CRITICAL ERROR: FLOOR DATA FOR " + "fd_" + towerID + "_" + chosenFloor + " COULD NOT BE FOUND. PLEASE MAKE SURE THE FLOORDATA FOR THIS FLOOR IS CONTAINED IN TOWERDATAHOLDER OR TALK TO MATT");
        }
        else
        {
            Debug.Log(floorData.name);

            manager.enemyTypes = floorData.enemyKinds;
            manager.possibleSpawnsObjects = floorData.possibleSpawnLocations;
            manager.pointsOfInterestObjects = floorData.pointsOfInterest;
            manager.pointOfInterestID = floorData.pointOfInterestID;
            heightAdjustment = floorData.heightAdjustment;
        }

        //Get a value for where the next floor should spawn
        float spawnHeight = bottomOfCurrentStage - heightAdjustment;
        Debug.Log(spawnHeight);

        //Create new floor using data that's found
        manager.spawnHeight = spawnHeight;
        manager.currentStageObject = Instantiate(floorData.prefab, new Vector3(0, bottomOfCurrentStage, 0), Quaternion.identity);

        //Create an object and then parent and move the floor
        GameObject centreDummy = new GameObject();
        centreDummy.transform.position = Vector3.zero;
        manager.currentStageObject.transform.SetParent(centreDummy.transform);
        manager.currentStageObject.transform.SetPositionAndRotation(new Vector3(0, manager.currentStageObject.transform.position.y, 0), Quaternion.identity);
        manager.currentStageObject.transform.SetParent(null);

        //Get the height measurement for the current stage
        bottomOfCurrentStage += (floorData.wallReferenceForHeight.GetComponent<MeshRenderer>().bounds.size.y + heightAdjustment) * -1 ;
        Debug.Log(bottomOfCurrentStage);

        if (chosenFloor == 3)
            chosenFloor = 0;
        else
            chosenFloor++;
    }


    //PROBABLY USELESS. HERE FOR REFERENCE
    /*string assetString = "fd";

       string folder = "Levels/";

       switch ((int)currentTower)
       {
           case 1:
               assetString += "_1";
               folder += "Tower1Data/";
               break;
           case 2:
               assetString += "_2";
               folder += "Tower2Data/";
               break;
           case 3:
               assetString += "_3";
               folder += "Tower3Data/";
               break;
       }

       switch (chosenFloor)
       {
           case 1:
               assetString += "_1";
               break;
           case 2:
               assetString += "_2";
               break;
           case 3:
               assetString += "_3";
               break;
           case 4:
               assetString += "_4";
               break;
           case 5:
               assetString += "_5";
               break;
               //And so on
       }

       folder += (assetString);

       folder.Trim();



       Object[] searchResults = Resources.FindObjectsOfTypeAll(typeof(FloorData));

       for (int i = 0; i < searchResults.Length; i++)
       {

           if (searchResults[i].name == assetString)
           {
               Debug.Log("Found asset " + assetString); //+ " at " + (string)AssetDatabase.GetAssetOrScenePath(searchResults[i]));

               floorData = Resources.Load<FloorData>(folder);

               dataFound = true;
               break;
           }


       }
       */




}


