using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ScrollSpeed = 15f;

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        if(x != 0 || y != 0)
        {
            Vector3 move = new Vector3(x, y, 0f).normalized;
            transform.position += move * ScrollSpeed * Time.deltaTime;
        }
    }
}
