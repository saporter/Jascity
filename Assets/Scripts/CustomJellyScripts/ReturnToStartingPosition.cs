using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToStartingPosition : MonoBehaviour
{
    Vector3 position;
    Quaternion rotation;
    Vector3 scale;
    Rigidbody2D rb;
    RigidbodyType2D bodyType;

    // Start is called before the first frame update
    private void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        bodyType = rb.bodyType;
    }

    private void Start()
    {
        GameManager.Instance.GameStateChange.AddListener(ResetPosition);
    }

    public void ResetPosition(GameState state)
    {
        if(state == GameState.Transition)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
            rb.bodyType = bodyType;
            rb.angularVelocity = 0f;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.GameStateChange.RemoveListener(ResetPosition);
        }
    }
}
