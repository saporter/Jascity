using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    
    WarbleActions actions;

    private void Awake()
    {
        actions = GetComponent<WarbleActions>();
    }

    void FixedUpdate()
    {
        float run = Input.GetAxis("Horizontal");
        float jump = Input.GetAxis("Jump");
        float shock = Input.GetAxis("Fire1");

        if (jump != 0)
        {
            actions.JumpTowards(actions.UpVector);
        }

        if (run != 0f)
        {
            actions.Run(run);
        }

        if(shock != 0f)
        {
            actions.Shock();
        }
    }

    

}
