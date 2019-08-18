using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform Target;

    private Vector2 extents;

    private void Awake()
    {
        var collider = GetComponent<BoxCollider2D>();
        extents = collider.bounds.extents;
    }

    private void Start()
    {
        GameManager.Instance.GameStateChange.AddListener(StopFollowing);
    }

    private void LateUpdate()
    {
        if (Target == null)
            return;

        // x
        var distance = Target.position.x - transform.position.x;
        if (Mathf.Abs(distance) > extents.x)
        {
            distance = distance > 0 ? distance - extents.x : distance + extents.x; 
            Vector3 newPos = (Vector2)transform.position + new Vector2(distance, 0f); 
            newPos.z = transform.position.z;
            transform.position = newPos;
        }

        // y
        distance = Target.position.y - transform.position.y;
        if (Mathf.Abs(distance) > extents.y)
        {
            distance = distance > 0 ? distance - extents.y : distance + extents.y;
            Vector3 newPos = (Vector2)transform.position + new Vector2(0f, distance);
            newPos.z = transform.position.z;
            transform.position = newPos;
        }

    }

    public void StopFollowing(GameState state)
    {
        if(state == GameState.Transition || state == GameState.Lab)
        {
            Target = null;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameStateChange.RemoveListener(StopFollowing);
        }
    }
}
