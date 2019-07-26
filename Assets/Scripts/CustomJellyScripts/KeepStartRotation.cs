using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepStartRotation : MonoBehaviour
{
    private Quaternion startRot;

    private void Awake()
    {
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = startRot;
    }
}
