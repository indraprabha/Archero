using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;

    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    // FixedUpdate is called on every physics update 
    void FixedUpdate()
    {
        Vector3 targetCameraPostion = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCameraPostion, smoothing * Time.deltaTime);
    }
}
