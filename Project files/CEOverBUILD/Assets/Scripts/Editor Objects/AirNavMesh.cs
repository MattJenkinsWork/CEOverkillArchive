using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AirNavMesh", menuName = "Custom /AirNavMesh", order = 3)]
public class AirNavMesh : ScriptableObject {

    //Custom asset for holding an airnavmesh

    //World positions of every node in the mesh
    public List<Vector3> worldPos = new List<Vector3>();

    //The maximum distance that nodes must be under to connect
    public float connectionRange = 0;

    //The list of nodes in the mesh. Filled when CreateNavMeshData is called. This has to be done at runtime because unity doesn't like serialising custom classes
    public List<Node> nodes = new List<Node>();

    //Each list here represents a node's linked nodes. This has to be done at runtime because unity doesn't like serialising custom classes
    public List<List<Node>> connectedNodes = new List<List<Node>>();

    //Creates a list of list of nodes that can be serialised. Used primarily for previewing navmeshes
    public void CreateConnectedNodes()
    {
        
        connectedNodes.Clear();

        for (int i = 0; i < nodes.Count; i++)
        {
            List<int> ids = nodes[i].nodeList.InnerList;
            List<Node> connected = new List<Node>();

            foreach (int id in ids)
            {
                connected.Add(nodes[id]);
            }

            connectedNodes.Add(connected);
        }
        
    }


    //Creates a node list at runtime from the data given. Called when the navmesh is swapped via the navmesh manager
    public void CreateNavMeshData()
    {
        nodes.Clear();

        for (int i = 0; i < worldPos.Count; i++)
        {
            Node node = new Node();
            node.moveCost = Random.Range(0, 3);
            node.worldPos = worldPos[i];
            node.ID = i;
            nodes.Add(node);
            node.nodeList = NodeConnectionCheck(nodes[i], connectionRange);
        }

    }

    //Creates a list of connections for the given node
    ListWrapper NodeConnectionCheck(Node currentNode, float range)
    {
        List<int> connected = new List<int>();

        for (int m = 0; m < nodes.Count; m++)
        {
            if(Vector3.Distance(currentNode.worldPos,nodes[m].worldPos) < range && !connected.Contains(currentNode.ID))
            {
                connected.Add(nodes[m].ID);

                if (!nodes[m].nodeList.InnerList.Contains(currentNode.ID))
                {
                    nodes[m].nodeList.InnerList.Add(currentNode.ID);
                }

            }

        }

        ListWrapper wrap = new ListWrapper();

        wrap.InnerList = connected;

        return wrap;
    }


    static bool NullChecker(int id)
    {
        if (id == -1)
            return true;
        else
            return false;
    }


}




















    /*

    public List<float> moveCost = new List<float>();
    public List<int> nodeID = new List<int>();
    public List<ListWrapper> connectedNodeID = new List<ListWrapper>();

    */
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    /*
    public Node IDtoNode(int id)
    {
        Node node = new Node();

        node.moveCost = moveCost[id];
        node.worldPos = worldPos[id];
        node.ID = nodeID[id];

        //Debug.Log(id + " " + connectedNodeID.Count);

        node.connectedNodesID = connectedNodeID[id].InnerList;

        return node;
    }

    public List<Node> CreateNavmesh()
    {
        List<Node> nodes = new List<Node>();

        for (int i = 0; i < nodeID.Count; i++)
        {
            Node node = new Node();

            node.moveCost = moveCost[i];
            node.worldPos = worldPos[i];
            node.ID = nodeID[i];
            node.connectedNodesID = connectedNodeID[i].InnerList;

            nodes.Add(node);

            return nodes;
        }

        return null;
    }
    */


