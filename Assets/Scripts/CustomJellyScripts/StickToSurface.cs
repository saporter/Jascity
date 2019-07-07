using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToSurface : MonoBehaviour
{
    Rigidbody2D lastToTouch;
    JellySprite j;
    float stickAgainTime = 0f;

    private void Awake()
    {
        j = GetComponent<JellySprite>();
    }

    void OnJellyCollisionEnter2D(JellySprite.JellyCollision2D collision)
    {
        if(!collision.Collision2D.otherCollider.IsTouchingLayers(LayerMask.GetMask("Floor")))
            return;
        if (Time.time < stickAgainTime)
            return;

        Unstick(0f);

        if (!j.m_LockRotation)
        {
            j.LockRotation(true);
        }

        lastToTouch = collision.ReferencePoint.GetComponent<Rigidbody2D>();
        lastToTouch.constraints |= RigidbodyConstraints2D.FreezePosition;   // stick
    }

    
    public void Unstick(float forSeconds)
    {
        stickAgainTime = Time.time + forSeconds;
        if (lastToTouch == null)
            return;

        lastToTouch.constraints &= ~RigidbodyConstraints2D.FreezePosition;
    }
}
