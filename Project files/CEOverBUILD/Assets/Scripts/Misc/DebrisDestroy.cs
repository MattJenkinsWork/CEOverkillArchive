using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisDestroy : MonoBehaviour {

    public float shaderUpdateWait = 0.01f;
    public float shaderDecreaseAmount = 0.001f;
    float dissolveAmount;
    int explosionForce = 1000;

    Material mat;

	// Use this for initialization
	void Start () {
        gameObject.layer = 29;

        try
        {
            mat = gameObject.GetComponent<Renderer>().material;
            StartCoroutine(DissolveShader());
        }
        catch
        {
            Destroy(gameObject, 10);
        }

        try
        {
            GetComponent<Rigidbody>().AddForce(Vector3.left * Random.Range(-explosionForce, explosionForce));
            GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(-explosionForce, explosionForce));
        }
        catch
        {

        }

       
    }
	
    //Dissolves the tile a little bit more after every runthrough
    IEnumerator DissolveShader()
    {
        yield return new WaitForSeconds(shaderUpdateWait);

        dissolveAmount += shaderDecreaseAmount;

        mat.SetFloat("_AmountOfDissolve", dissolveAmount);

        if(dissolveAmount > 0.75f)
        {
            Destroy(gameObject);
        }


        StartCoroutine(DissolveShader());



    }
}
