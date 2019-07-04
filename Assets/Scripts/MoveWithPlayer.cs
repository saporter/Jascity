using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{
    public float Speed = 0.02f;
    public bool Destructable = true;
    public Vector3 OutOfTheWay = new Vector3(-1000f, 1000f, 0f);

    private bool inPlay = false;
    private Vector3 startPos;
    private Quaternion startRot;
    private Transform[] children;
    private Vector3[] startPositions;
    
    private void Awake()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        children = GetComponentsInChildren<Transform>();

        List<Vector3> pos = new List<Vector3>();
        for (int i = 0; i < children.Length; ++i)
        {
            pos.Add(children[i].position);
        }
        startPositions = pos.ToArray();
    }

    private void Start()
    {
        GameManager.Instance.RacingToggleEvent.AddListener(RacingToggle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CameraViewTrigger")
        {
            StopPlaying();
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.RacingToggleEvent.RemoveListener(RacingToggle);
        }
    }

    private void RacingToggle()
    {
        if (GameManager.Instance.Racing)
        {
            inPlay = true;
            StartCoroutine(Moving());
        }
        else
        {
            StopPlayingAndMoveToStart();
        }
    }

    private void StopPlayingAndMoveToStart()
    {
        inPlay = false;

        transform.localPosition = startPos;
        transform.localRotation = startRot;

        for (int i = 0; i < children.Length; ++i)
        {
            if (children[i] != transform)
            {
                children[i].position = startPositions[i];
            }

        }
    }

    public void StopPlaying()
    {
        inPlay = false;

        transform.position -= OutOfTheWay;
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
                transform.localPosition += Vector3.down * Speed;
            }
            yield return new WaitForFixedUpdate();
        }

    }
}
