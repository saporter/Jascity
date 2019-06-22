using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float Speed = 5f;    // How fast the track moves
    public Transform CameraRacingPosition;
    public bool Racing { get; private set; }
    public GameObject RacingWarble { get; private set; }

    private Vector3 startCameraPos;
    private Vector3 startPos;
    private Quaternion startRotation;
    private Timer timer;
    private InputController input;

    private void Awake()
    {
        Racing = false;
        startCameraPos = Camera.main.transform.position;
        startPos = transform.position;
        startRotation = transform.rotation;
        timer = GetComponent<Timer>();
        input = GetComponent<InputController>();

        if (Instance != null)
            Debug.LogError("Two GameManagers in scene");
        Instance = this;
    }

    private void Update()
    {
        if(Racing && Input.GetAxis("Cancel") > 0f)
        {
            ResetRace();
        }
    }

    public void ResetRace()
    {
        if (!Racing)
            return;

        Racing = false;
        transform.position = startPos;
        transform.rotation = startRotation;
        Camera.main.transform.position = startCameraPos;
        RacingWarble.transform.position = startPos;
        RacingWarble.transform.SetParent(null);
        RacingWarble = null;
        timer.StopTimer();
        input.ActiveWarble = null;
    }

    public void SetupRace(GameObject warble)
    {
        Racing = true;
        RacingWarble = warble;
        RacingWarble.transform.SetParent(transform);
        StartCoroutine(MoveCameraToWarble());
    }

    public void StartRace()
    {
        if (!Racing)
            return;
        timer.StartTimer();
        input.ActiveWarble = RacingWarble;
        StartCoroutine(RaceAroundTrack());
    }

    IEnumerator MoveCameraToWarble()
    {
        Vector3 end = CameraRacingPosition.position;//new Vector3(RacingWarble.transform.position.x, RacingWarble.transform.position.y, Camera.main.transform.position.z);
        Vector3 v = Vector3.zero;

        while (Vector3.Distance(Camera.main.transform.position, end) > 0.005f && Racing)
        {
            end = CameraRacingPosition.position; //new Vector3(RacingWarble.transform.position.x, RacingWarble.transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, end, ref v, 0.5f);
            
            yield return new WaitForSeconds(0.02f);
        }
        Debug.Log("Done moving camera");
    }

    IEnumerator RaceAroundTrack()
    {
        while(Racing)
        {
            // TODO implement this
            if(timer.TimeLeft <= 0f)
            {
                ResetRace();
                break;
            }

            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * Speed, 0.02f);

            yield return new WaitForFixedUpdate();
        }
    }
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
}
