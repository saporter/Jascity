using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbleActions : MonoBehaviour
{
    public float JumpForce = 1000f;
    public float JumpDelay = 0.1f;
    public float MaxVelocity = 30f;
    public float Speed = 200f;
    public LayerMask GroundLayer; //= LayerMask.GetMask("Floor");
    public ShockController Lightning;
    public Vector2 ForwardMovementDirection { get { return moveDirection; } set { moveDirection = value; pushToFloor = -Vector2.Perpendicular(value); UpVector = -pushToFloor; } }
    public Vector2 UpVector { get; private set; }
    public bool PauseActions { get; set; }

    Vector2 moveDirection = Vector2.right;
    Vector2 pushToFloor = -Vector2.Perpendicular(Vector2.right);
    StickToSurface sticky;
    JellySprite jelly;
    CircleCollider2D trigger;

    private void Awake()
    {
        sticky = GetComponent<StickToSurface>();
        jelly = GetComponent<JellySprite>();
        UpVector = Vector2.Perpendicular(ForwardMovementDirection);
        trigger = GetComponent<CircleCollider2D>();
        PauseActions = false;
    }

    private void Start()
    {
        jelly.LockRotation(true);
    }

    private float lastJumpTime;
    public void JumpTowards(Vector2 direction)
    {
        if (PauseActions)
            return;
        if (Time.time - lastJumpTime < JumpDelay)
            return;
        if (!IsGrounded())
            return;

        lastJumpTime = Time.time;
        jelly.LockRotation(false);
        sticky.Unstick(0.1f);
        jelly.AddForce(direction.normalized * JumpForce);
    }

    public void Run(float input)
    {
        if (PauseActions)
            return;

        if (input != 0f && IsGrounded())
        {
            if (jelly.m_LockRotation)
            {
                jelly.LockRotation(false);
            }
            if (jelly.CentralPoint.Body2D.velocity.magnitude < MaxVelocity)
            {
                CheckCornerAndUpdateForward(input);
                var move = ForwardMovementDirection.normalized * input + pushToFloor * Mathf.Abs(input);
                jelly.AddForce(move * Speed);
            }

            
        }
    }

    void CheckCornerAndUpdateForward(float input)
    {
        var direction = input > 0 ? ForwardMovementDirection : -ForwardMovementDirection;
        var hit = Physics2D.Raycast(transform.position, direction, trigger.radius, GroundLayer);

        if (hit.collider != null)
        {
            ForwardMovementDirection = hit.collider.gameObject.GetComponent<ChangeHorizontalVector>().HorizontalRight;
        }
    }

    public void ChargeShock()
    {
        if (PauseActions)
            return;

        Lightning.Charge();
    }

    public bool IsGrounded()
    {
        return jelly.IsGrounded(LayerMask.GetMask("Floor"), 1);
    }

}

