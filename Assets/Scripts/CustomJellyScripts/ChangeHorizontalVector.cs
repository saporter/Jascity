using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHorizontalVector : MonoBehaviour
{
    public Vector2 HorizontalRight = Vector2.right;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var jelly = collision.GetComponent<WarbleActions>();
        if(jelly != null)
        {
            jelly.ForwardMovementDirection = HorizontalRight;
            //jelly.Floors.Add(this);
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    var jelly = collision.GetComponent<WarbleActions>();
    //    if (jelly != null)
    //    {
    //        jelly.Floors.Add(this);
    //    }
    //}
}
