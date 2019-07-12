using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    GeneBehaviours controller;

    private void Awake()
    {
        controller = GetComponent<GeneBehaviours>();
    }

    private void Update()
    {
        if(Input.GetAxis("Jump") != 0)
        {
            controller.JumpTowards(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetAxis("Fire1") != 0)
        {
            controller.Running(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1f);
        }

        if(Input.GetAxis("Fire2") != 0)
        {
            controller.Shock();
            controller.Running(Camera.main.ScreenToWorldPoint(Input.mousePosition), -1f);
        }
    }
}
