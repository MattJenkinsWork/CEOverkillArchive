using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFall : MonoBehaviour {

    //the minimum and maximum amount of time the tile should wait before dropping
    public float maxWaitTime = 7;
    public float minWaitTime = 5;

    //The time the tile will destroy after
    public float destroyTime = 10;

    //How much downwards velocity should the tile get upon falling
    public float initialVelocity = 100;

    //Defines whether the tile will fall. It will remain stationary if this is false
    public bool willFall = true;

    //How long will the shader wait between updates. Raising this will make the shader take longer to complete
    public float shaderUpdateWait;

    //How much the shader dissolves by for each shader update
    public float shaderDecreaseAmount;

    Material mat;
    float dissolveAmount;

    public void Fall()
    {
        if (willFall)
        {
            mat = gameObject.GetComponent<Renderer>().material;
            StartCoroutine(FallC());
            StartCoroutine(DelayedDestroy());
        }
            
    }




	public IEnumerator FallC()
    {
        

        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        StartCoroutine(DisableCollider());
        StartCoroutine(DissolveShader());

        gameObject.layer = 29;


        Rigidbody rigid = gameObject.AddComponent<Rigidbody>();
        rigid.AddForce(Vector3.down * initialVelocity);
        rigid.AddForce(Vector3.left * Random.Range(-100, 100));


    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    //Disables the colliders on the hex tiles briefly so they can fall properly
    IEnumerator DisableCollider()
    {
        Destroy(gameObject.GetComponent<BoxCollider>());

        yield return new WaitForSeconds(0.4f);

       // MeshCollider meshCol = gameObject.AddComponent<MeshCollider>();
        //meshCol.convex = true;
       // meshCol.gameObject.layer = 29;

    }

    //Dissolves the tile a little bit more after every runthrough
    IEnumerator DissolveShader()
    {
        yield return new WaitForSeconds(shaderUpdateWait);

        dissolveAmount += shaderDecreaseAmount;

        mat.SetFloat("_AmountOfDissolve", dissolveAmount);

        StartCoroutine(DissolveShader());
    }
}
