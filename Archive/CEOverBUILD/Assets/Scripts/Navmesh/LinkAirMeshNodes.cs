using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkAirMeshNodes : MonoBehaviour {

    //NOW DEPRECATED, USE AIRMESHGENERATOR INSTEAD

    /*

    public float distanceTolerance;
    List<Node> nodes = new List<Node>();
    public List<Vector3> globalPos = new List<Vector3>();

    public bool useMesh;
    public AirNavMesh meshTest;

    private void Awake()
    {
        if (useMesh)
            globalPos = meshTest.meshPoints;

        BuildNodes();
    }


    void BuildNodes()
    {
        for (int i = 0; i < globalPos.Count; i++)
        {
            //if (globalPos[i] == Vector3.zero)
            //    continue;

            Node node = new Node();
            node.moveCost = Random.Range(0, 2);
            node.worldPos = globalPos[i];
            node.ID = i;
            nodes.Add(node);
            node.connectedNodes = NodeConnectionCheck(nodes[i], distanceTolerance);

            //Debug.Log(nodes[i].connectedNodes[0].ID);

        }
    }

    //Find other nodes by ID, then compare distance
    List<Node> NodeConnectionCheck(Node node , float meshSpread)
    {

        List<Node> connected = new List<Node>();

        for (int i = 0; i < nodes.Count; i += 2)
        {
            for (int m = -1000; m < 1000; m++)
            {
                connected.Add(CheckAddNodes(m, i, node));
            }

        }

        connected.RemoveAll(NullChecker);

        return connected;
    }

    static bool NullChecker(Node node)
    {
        if (node == null)
            return true;
        else
            return false;
    }


    Node CheckAddNodes(int IDmod,int nodeIndex, Node currentNode)
    {
        if(nodes[nodeIndex].ID == currentNode.ID + IDmod && Vector3.Distance(nodes[nodeIndex].worldPos, currentNode.worldPos) <= distanceTolerance)
        {
            if (!nodes[nodeIndex].connectedNodes.Contains(currentNode))
            {
                nodes[nodeIndex].connectedNodes.Add(currentNode);
            }         
            return nodes[nodeIndex];
        }
        else
        {
            return null;
        }
    }


    void ResetPathfinding()
    {

    }

    void FindGreedyPath()
    {

    }

    


    private void OnDrawGizmos()
    {
       // Debug.Log(nodes.Count);
        //Debug.DrawLine(nodes[1].connectedNodes[0].worldPos, nodes[0].worldPos);
        //Debug.Log(nodes[1].connectedNodes[0].worldPos);

        for (int m = 0; m < nodes.Count; m++)
        {
            for (int i = 0; i < nodes[m].connectedNodes.Count; i++)
            {
                //Debug.Log("Drwe line");
                Debug.DrawLine(nodes[m].connectedNodes[i].worldPos, nodes[m].worldPos,Color.black);
            }
        }

        try
       {

            //Debug.Log(nodes.Count);
            //Debug.Log(nodes[1].worldPos);

            

  
            
            

            


       }
       catch
        {
           
        }


        
        


        /*foreach (Node node in nodes)
        {
            for (int i = 0; i < node.connectedNodes.Count; i++)
            {
                Debug.DrawLine(node.connectedNodes[i].worldPos,node.worldPos);
            }

            
        }
    }

    public class Node
    {
        public float moveCost;
        public Vector3 worldPos;
        public int ID;
        public List<Node> connectedNodes = new List<Node>();
    }

    */
}

