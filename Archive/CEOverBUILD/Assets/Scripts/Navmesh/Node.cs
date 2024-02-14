using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    //Move cost to move to this node. Currently unused
    public float moveCost;

    //The world position of this node
    public Vector3 worldPos;

    //The ID of this node. No other node will have the same id
    public int ID;

    //A listwrapper with a list of nodes inside
    public ListWrapper nodeList = new ListWrapper();
}


