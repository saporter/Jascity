using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHorizontalVector : MonoBehaviour
{
    public Vector2 HorizontalRight = Vector2.zero;

    private void Awake()
    {
        if (HorizontalRight == Vector2.zero)
        {
            HorizontalRight = transform.right.normalized; 
        }

        HorizontalRight = HorizontalRight.normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var jelly = collision.GetComponent<WarbleActions>();
        if(jelly != null)
        {
            jelly.ForwardMovementDirection = HorizontalRight;
        }
    }
}
