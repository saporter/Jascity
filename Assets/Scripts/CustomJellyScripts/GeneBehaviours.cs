using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneBehaviours : MonoBehaviour
{
    public float RunLookout = 20f;
    WarbleActions actions;

    private void Awake()
    {
        actions = GetComponent<WarbleActions>();
    }

    public void JumpTowards(Vector2 target)
    {
        Vector2 jumpTo = target - (Vector2)transform.position;
        actions.JumpTowards(jumpTo);
    }

    public void Running(Vector2 target, float input)
    {
        Vector2 position = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(position, actions.ForwardMovementDirection, RunLookout, LayerMask.GetMask("Floor"));
        float rlookout = hit.collider == null ? RunLookout : hit.distance;
        hit = Physics2D.Raycast(position, -actions.ForwardMovementDirection, RunLookout, LayerMask.GetMask("Floor"));
        float llookout = hit.collider == null ? RunLookout : hit.distance;

        Vector2 right = position + actions.ForwardMovementDirection * rlookout;
        Vector2 left = position - actions.ForwardMovementDirection * llookout;
        if ((target - right).magnitude > (target - left).magnitude)
        {
            actions.Run(input);
        }
        else
        {
            actions.Run(-input);
        }
    }

    public void Shock()
    {
        actions.ChargeShock();
    }

}
