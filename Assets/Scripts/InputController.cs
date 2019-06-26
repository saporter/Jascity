using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float Speed = 5f;
    public float TurboFactor = 3f;
    public GameObject ActiveWarble;             // Set by GM

    private float normalSpeed;

    private void Awake()
    {
        normalSpeed = Speed;
    }

    void Update()
    {
        if (ActiveWarble == null)
            return;

        var input = Input.GetAxis("Horizontal");
        var turbo = Input.GetAxis("Vertical");

        if(input != 0)
        {
            ActiveWarble.transform.localPosition += Vector3.right * input * Speed * Time.deltaTime;
        }
        if(turbo != 0)
        {
            Time.timeScale = TurboFactor;
            Speed = normalSpeed / TurboFactor; // horizontal move remains constant
        }
        else
        {
            Time.timeScale = 1f;
            Speed = normalSpeed;
        }
    }
}
