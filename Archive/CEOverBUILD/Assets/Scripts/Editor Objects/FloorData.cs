using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorData", menuName = "Custom /Floor Data", order = 1)]
public class FloorData : ScriptableObject
{

    //These are all parameters for the floor generation system
    //They provide info for how the current floor should function

    public GameObject[] enemyKinds = new GameObject [1];

    public int[] amountOfEnemyToSpawn = new int[1];

    public int[] maxAmountOfEnemyInField = new int[1];


}

