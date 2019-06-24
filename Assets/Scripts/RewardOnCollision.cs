using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardOnCollision : MonoBehaviour
{
    public float Reward = 7f;
    private MoveWithPlayer food;
    private Timer timer;
    private float timeAwarded;  // A helper to stop the multiple calls to OnTriggerEnter and alot the reward only once

    private void Awake()
    {
        food = GetComponent<MoveWithPlayer>();
    }

    private void Start()
    {
        timer = GameManager.Instance.GetComponent<Timer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.RacingWarble)
        {
            if (Time.time - timeAwarded > 0.2f)
            {
                timeAwarded = Time.time;
                timer.AdditionalTime += Reward;
                food.StopPlayingAndMoveToStart();
            }
        }
    }
}
