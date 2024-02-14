using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexFloorMaker))]
public class HexGenerator : Editor
{
    // The Number of tiles
    private int height = 10;
    private int width = 10;

    private Vector3 rowStart;

	private float hDistance = 3;
    private float vDistance = 0.9f;
    private float rOffset = 1.5f;

    private GameObject parent;

	private Object hexTile;

	bool alignLeft;

    List<GameObject> hexes;

    public override void OnInspectorGUI()
    {

        GUILayout.Label("~~~~ Hex Floor Settings ~~~~");

        GUILayout.Label("Z is vertical on this grid");

        //Creating fields
        height = EditorGUILayout.IntField("Vertical Hexes", height);
        width = EditorGUILayout.IntField("Horizontal Hexes", width);

        rowStart = EditorGUILayout.Vector3Field("Start Position", rowStart);

        hDistance = EditorGUILayout.FloatField("Distance between hexes on a row", hDistance);
        vDistance = EditorGUILayout.FloatField("Row Height Gap", vDistance);

        rOffset = EditorGUILayout.FloatField("Row Offset", rOffset);

        hexTile = EditorGUILayout.ObjectField("Tile Prefab", hexTile, typeof(GameObject), false);

        //Button functions
        if (GUILayout.Button("Build Floor"))
            Generate();

        if (GUILayout.Button("Remove last floor"))
            RemoveHexes();

    }


	void Generate ()
	{
        Vector3 rowStore;
        rowStore = rowStart;

        parent = new GameObject();
        parent.name = "Hex Grid";
        parent.tag = "Floor";

        hexes = new List<GameObject>();
        hexes.Clear();

		for (int I = 0; I < height; I++) {

			if (alignLeft == false)
            {
				rowStart = new Vector3(rowStart.x + rOffset,rowStart.y,rowStart.z);
				alignLeft = !alignLeft;
			}
            else if (alignLeft == true)
            {
				rowStart = new Vector3(rowStart.x - rOffset, rowStart.y, rowStart.z);
                alignLeft = !alignLeft;
			}

			for (int i = 0; i < width; i++)
            {

                GameObject hex = (GameObject)Instantiate(hexTile,new Vector3(rowStart.x + (hDistance * i), rowStart.y,rowStart.z), Quaternion.identity);
                hex.transform.SetParent(parent.transform);
				hex.name = "Hex" + I + "_" + i;
                hex.AddComponent<TileFall>();
                hex.GetComponent<MeshCollider>().convex = true;
                hexes.Add(hex);

			}

            rowStart = new Vector3(rowStart.x, rowStart.y, rowStart.z + vDistance);

		}

        rowStart = rowStore;
	}

    void RemoveHexes()
    {
        for (int i = 0; i < hexes.Count; i++)
        {
            DestroyImmediate(hexes[i]);
            DestroyImmediate(parent);
        }

    }
}
