using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForthMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform leftPoint;
    public Transform rightPoint;
    public float MaxVelocity = 100f;
    public float MoveForce = 100f;

    private bool movingLeft = true;

    private void FixedUpdate()
    {
        var direction = movingLeft ? Vector2.left : Vector2.right;
    }

}
