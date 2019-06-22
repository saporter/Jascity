using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    public float RotationMaxSpeed = 3f;
    private float rotX, rotY, rotZ;

    private void Awake()
    {
        rotX = Random.Range(-RotationMaxSpeed, RotationMaxSpeed);
        rotY = Random.Range(-RotationMaxSpeed, RotationMaxSpeed);
        rotZ = Random.Range(-RotationMaxSpeed, RotationMaxSpeed);
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        transform.RotateAround(transform.position, Vector3.up, rotY);
        transform.RotateAround(transform.position, Vector3.left, rotX);
        transform.RotateAround(transform.position, Vector3.forward, rotZ);
    }
}
