using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Light WorldLight;
    public ParticleSystem WarpParticleSystem;
    public WarblesReactToThis MouseTrigger;
    public Light MouseSpotLight;
    public CageManager WarbleCageManager;
    public GameState CurrentGameState { get { return currentGameState; } }
    public Transform CavePosition;
    public Transform LabPosition;

    public GameStateEvent GameStateChange = new GameStateEvent();

    private GameState currentGameState = GameState.Lab;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Two GameManagers in scene");
        Instance = this;
    }

    private void Start()
    {
        GameStateChange.AddListener(ChangingStateListener);
    }

    private void ChangingStateListener(GameState state)
    {
        currentGameState = state;
        switch(state)
        {
            case GameState.Lab:
                Camera.main.GetComponent<CameraController>().enabled = false;
                MouseTrigger.enabled = false;
                MouseSpotLight.enabled = false;
                WarpParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                break;
            case GameState.Cave:
                Camera.main.GetComponent<CameraController>().enabled = true;
                MouseTrigger.enabled = true;
                MouseSpotLight.enabled = true;
                WarpParticleSystem.Play();
                break;
            case GameState.Maze:
                MouseTrigger.enabled = false;
                MouseSpotLight.enabled = false;
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

    public static IEnumerator MoveCameraToPoint(Transform point, float travelTime)
    {
        Vector3 end = point.position;
        end.z = Camera.main.transform.position.z;
        Vector3 v = Vector3.zero;
        float start = Time.time;
        float buffer = 4f;

        while (Vector3.Distance(Camera.main.transform.position, end) > 0.1f && (Time.time - start < travelTime))
        {
            end = point.position;
            end.z = Camera.main.transform.position.z;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, end, ref v, travelTime / buffer);

            yield return new WaitForEndOfFrame();
        }

        Camera.main.transform.position = end;
    }

}