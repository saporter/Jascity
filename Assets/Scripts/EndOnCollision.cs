using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOnCollision : MonoBehaviour
{
    private MoveWithPlayer obstacle;

    private void Awake()
    {
        obstacle = GetComponent<MoveWithPlayer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.RacingWarble)
        {
            GameManager.Instance.ResetRace();
            obstacle.StopPlayingAndMoveToStart();
        }
    }
}
