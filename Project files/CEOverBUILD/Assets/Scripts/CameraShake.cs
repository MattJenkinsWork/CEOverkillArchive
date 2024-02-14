using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration;
    public float shakeDurationHolder;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            //camTransform = transform.GetChild(0).GetComponent(typeof(Transform)) as Transform;
            camTransform = GetComponent(typeof(Transform)) as Transform;

        }
        //shakeDurationHolder = shakeDuration;
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = shakeDurationHolder;
            camTransform.localPosition = originalPos;
        }
    }
}
