using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    public float RotateSpeed = 5f;
    public bool NodeReached { get; private set; }

    private void Awake()
    {
        NodeReached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="CameraViewTrigger")
        {
            NodeReached = true;
        }
    }
}
