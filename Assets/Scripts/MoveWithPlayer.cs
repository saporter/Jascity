using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{
    public float Speed = 0.02f;

    private bool inPlay = true;
    private Vector3 startPos;
    private Quaternion startRot;

    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CameraViewTrigger")
        {
            if (transform.parent != GameManager.Instance.transform)
            {
                StartPlaying();
            }
            else
            {
                StopPlayingAndMoveToStart();
            }
        }
    }

    public void StartPlaying()
    {
        transform.SetParent(GameManager.Instance.transform);
        inPlay = true;
        StartCoroutine(Moving());
    }

    public void StopPlayingAndMoveToStart()
    {
        transform.SetParent(null);
        inPlay = false;

        transform.position = startPos;
        transform.rotation = startRot;
    }

    IEnumerator Moving()
    {
        while (inPlay)
        {
            // Second check in case game was stopped from elsewhere
            if (!GameManager.Instance.Racing)
            {
                StopPlayingAndMoveToStart();
            }
            // Otherwise proceed as normal
            else
            {
                transform.position += Vector3.down * Speed;
            }
            yield return new WaitForFixedUpdate();
        }

    }
}
