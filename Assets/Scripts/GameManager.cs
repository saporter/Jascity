using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float Speed = 5f;    // How fast the track moves
    public float RotationSpeed = 5f;
    public Transform CameraRacingPosition;
    public Transform CameraFinalPosition;
    public float MaxCameraViewSize = 450f;
    public GameObject RacingPathNodesParent;
    public float MinDistanceToNode = 1f; // How close to the node before moving on to the next one
    public GameObject StartAreaGameObject;
    public Transform OffScreenLocation;
    public bool Racing { get; private set; }
    public bool Finished { get; private set; }
    public GameObject RacingWarble { get; private set; }
    public CanvasGroup NotepadCG;
    public UnityEvent RacingToggleEvent;
    
    private Vector3 startCameraPos;
    private Quaternion startCameraRot;
    private float startCameraSize;
    private Transform startCameraParent;
    private Vector3 startPos;
    private Quaternion startRotation;
    private Timer timer;
    private InputController input;
    private List<Transform> pathNodes;
    private int currentPosition;
    private Transform originalWarbleParent;
    private Vector3 originalStartAreaPosition;
    private bool notepadOn = false;   // an admin flag that turns the notepad back on once off

    private void Awake()
    {
        Racing = false;
        Finished = false;
        startCameraPos = Camera.main.transform.position;
        startCameraRot = Camera.main.transform.rotation;
        startCameraSize = Camera.main.orthographicSize;
        startCameraParent = Camera.main.transform.parent;
        startPos = transform.position;
        startRotation = transform.rotation;
        timer = GetComponent<Timer>();
        input = GetComponent<InputController>();
        pathNodes = new List<Transform>(RacingPathNodesParent.GetComponentsInChildren<Transform>());
        pathNodes.Remove(RacingPathNodesParent.transform);
        currentPosition = 0;
        originalStartAreaPosition = StartAreaGameObject.transform.position;

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

    public void SetupRace(GameObject warble)
    {
        Racing = true;
        RacingWarble = warble;
        RacingWarble.transform.rotation = transform.rotation;
        RacingWarble.GetComponent<GeneBehaviorController>().enabled = true;
        originalWarbleParent = RacingWarble.transform.parent;
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
        RacingToggleEvent.Invoke();
        StartCoroutine(MoveStartAreaIn(1f));

        if (NotepadCG.alpha > 0f)
        {
            notepadOn = true;
            HelperTools.ToggleOff(NotepadCG);
        }
    }

    

    public void ResetRace()
    {
        if (!Racing)
            return;

        RacingToggleEvent.Invoke();
        StartAreaGameObject.transform.position = originalStartAreaPosition;
        Racing = false;
        Finished = false;
        transform.position = startPos;
        transform.rotation = startRotation;
        Camera.main.transform.position = startCameraPos;
        Camera.main.transform.rotation = startCameraRot;
        Camera.main.orthographicSize = startCameraSize;
        Camera.main.transform.SetParent(startCameraParent);
        RacingWarble.GetComponent<GeneBehaviorController>().enabled = false;
        RacingWarble.transform.position = startPos;
        RacingWarble.transform.SetParent(originalWarbleParent);
        RacingWarble = null;
        timer.StopTimer();
        input.ActiveWarble = null;
        currentPosition = 0;

        if (notepadOn)
        {
            HelperTools.ToggleOn(NotepadCG);
        }
    }

    IEnumerator MoveCameraToWarble()
    {
        Vector3 end = CameraRacingPosition.position;
        Vector3 v = Vector3.zero;
        float start = Time.time;

        while (Vector3.Distance(Camera.main.transform.position, end) > 0.05f && Racing && (Time.time - start < 1f))
        {
            end = CameraRacingPosition.position; 
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, end, ref v, 0.4f);

            yield return new WaitForEndOfFrame();
        }

        Camera.main.transform.position = CameraRacingPosition.position;        
    }

    IEnumerator MoveCameraToFinalViewPosition()
    {
        Camera.main.transform.SetParent(CameraFinalPosition);
        Vector3 v = Vector3.zero;
        float start = Time.time;
        Camera m = Camera.main;

        while (Vector3.Distance(m.transform.position, Vector3.zero) > 0.05f && Racing)
        {
            m.transform.localPosition = Vector3.SmoothDamp(m.transform.localPosition, Vector3.zero, ref v, 4f);
            m.transform.localRotation = Quaternion.RotateTowards(m.transform.localRotation, m.transform.parent.localRotation, 0.3f);

            if (m.orthographicSize < MaxCameraViewSize)
            {
                m.orthographicSize += 1f;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator RaceAroundTrack()
    {
        while(Racing && !Finished)
        {
            if(timer.TimeLeft <= 0f)
            {
                ResetRace();
                break;
            }

            // Rotation
            Vector3 direction = (pathNodes[currentPosition].position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);

            // Movement
            transform.position = Vector3.MoveTowards(transform.position, pathNodes[currentPosition].position, Speed * Time.deltaTime);

            if (Vector3.Distance(pathNodes[currentPosition].position, transform.position) < MinDistanceToNode)
            {
                currentPosition++;
                if(currentPosition >= pathNodes.Count)
                {
                    Finished = true;
                    timer.PauseTimer();
                    StartCoroutine(MoveCameraToFinalViewPosition());
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator MoveStartAreaIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (Racing)
        {
            StartAreaGameObject.transform.position = OffScreenLocation.position;
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
