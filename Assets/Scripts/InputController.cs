using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float Speed = 5f;
    public GameObject ActiveWarble;             // Set by GM

    void Update()
    {
        if (ActiveWarble == null)
            return;

        var input = Input.GetAxis("Horizontal");

        if(input != 0)
        {
            ActiveWarble.transform.position += Vector3.right * input * Speed * Time.deltaTime;
        }
    }
}
