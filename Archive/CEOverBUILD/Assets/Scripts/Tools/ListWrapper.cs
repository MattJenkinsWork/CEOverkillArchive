using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Serves as a wrapper for the lists of connected nodes
//It's probably unneccesary tbh
[System.Serializable]
public class ListWrapper{

    //List of ids of nodes that this node is connected to
    public List<int> InnerList = new List<int>();

}
