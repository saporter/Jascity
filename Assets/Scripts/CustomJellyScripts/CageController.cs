using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CageController : MonoBehaviour
{
    public Button LaunchButton;
    public Rigidbody2D LeftHatch;
    public Rigidbody2D RightHatch;

    public MovementController Warble { get { return warble; } }

    MovementController warble;
    StickToSurface sticky;
    bool launchedWarble = false;

    private readonly float cameraTravelTime = 1f;

    private void Start()
    {
        GameManager.Instance.GameStateChange.AddListener(DisableWarble);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var movement = collision.gameObject.GetComponent<MovementController>();
        if (movement != null)
        {
            warble = movement;
            sticky = warble.GetComponent<StickToSurface>();
        }
        LaunchButton.interactable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LaunchButton.interactable = false;
    }

    public void OpenDoors()
    {
        StartCoroutine(HelperTools.MoveCameraToPoint(transform, cameraTravelTime));
        StartCoroutine(FollowWarble());

        launchedWarble = true;
        RightHatch.bodyType = RigidbodyType2D.Dynamic;
        LeftHatch.bodyType = RigidbodyType2D.Dynamic;
        warble.enabled = true;
        sticky.Unstick(1f);
    }
    
    IEnumerator FollowWarble()
    {
        yield return new WaitForSeconds(cameraTravelTime);
        Camera.main.GetComponent<FollowTarget>().Target = warble.transform;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.GameStateChange.Invoke(GameState.Maze);
    }

    public void DisableWarble(GameState state)
    {
        if(state == GameState.Transition && warble != null && launchedWarble)
        {
            warble.enabled = false;
            warble = null;
            launchedWarble = false;
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.GameStateChange.RemoveListener(DisableWarble);
        }
    }
}
