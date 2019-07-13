using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform CavePosition;
    public Transform LabPosition;
    public Transform MazePosition;

    public GameStateEvent GameStateChange = new GameStateEvent();

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Two GameManagers in scene");
        Instance = this;
    }

    private void Start()
    {
        GameStateChange.AddListener(ChangingState);
    }

    private void ChangingState(GameState state)
    {
        // TODO: Mouse and camera control stuff
        switch(state)
        {
            case GameState.Lab:
                Camera.main.GetComponent<CameraController>().enabled = false;
                break;
            case GameState.Cave:
                Camera.main.GetComponent<CameraController>().enabled = true;
                break;
            case GameState.Transition:
                Camera.main.GetComponent<CameraController>().enabled = false;
                break;
        }
    }

    private void OnDestroy()
    {
        GameStateChange.RemoveAllListeners();
    }
}

public class GameStateEvent : UnityEvent<GameState>
{

}

public enum GameState
{
    Lab,
    Cave,
    Maze,
    Transition
}

public class HelperTools
{
    public static void ToggleOn(CanvasGroup cg)
    {
        cg.alpha = 1.0f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public static void ToggleOff(CanvasGroup cg)
    {
        cg.alpha = 0.0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public static IEnumerator MoveCameraToPoint(Vector3 point, float travelTime)
    {
        Vector3 end = point;
        Vector3 v = Vector3.zero;
        float start = Time.time;
        float buffer = 4f;

        while (Vector3.Distance(Camera.main.transform.position, end) > 0.1f && (Time.time - start < travelTime))
        {
            //end = point;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, end, ref v, travelTime / buffer);

            yield return new WaitForEndOfFrame();
        }

        Camera.main.transform.position = point;
    }

}