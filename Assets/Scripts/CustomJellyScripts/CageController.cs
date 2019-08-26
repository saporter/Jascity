using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CageController : MonoBehaviour
{
    public Button LaunchButton;
    public Button KillButton;
    public Rigidbody2D LeftHatch;
    public Rigidbody2D RightHatch;
    public SwitchController Switch;
    public ParticleSystem KillParticleEffect;
    public DNAUIController DNAController;

    public MovementController Warble { get { return warble; } }

    MovementController warble;
    StickToSurface sticky;
    MovementController launchedWarble; // because something has to switch off player controlled movement.. so I'm just gonna do it here

    private readonly float cameraTravelTime = 1f;

    private void Start()
    {
        GameManager.Instance.GameStateChange.AddListener(DisableWarble);
        Switch.Enable(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var movement = collision.gameObject.GetComponent<MovementController>();
        if (movement != null)
        {
            warble = movement;
            DNAController.WarbleAdded(warble.GetComponent<Genes>().GeneSequence);
        }

        LaunchButton.interactable = true;
        KillButton.interactable = true;
        Switch.Enable(true);

        GameManager.Instance.WarbleBreedController.ToggleButton();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LaunchButton.interactable = false;
        KillButton.interactable = false;
        Switch.Enable(false);
        DNAController.DeleteCurrentDisplay();
        GameManager.Instance.WarbleBreedController.ToggleButton();
    }

    public void OpenDoors()
    {
        StartCoroutine(HelperTools.MoveCameraToPoint(transform, cameraTravelTime));

        launchedWarble = warble;
        RightHatch.bodyType = RigidbodyType2D.Dynamic;
        LeftHatch.bodyType = RigidbodyType2D.Dynamic;
        launchedWarble.enabled = true;
        sticky = launchedWarble.GetComponent<StickToSurface>();
        sticky.Unstick(1f);

        StartCoroutine(FollowWarble());
        warble = null;
    }
    
    IEnumerator FollowWarble()
    {
        yield return new WaitForSeconds(cameraTravelTime);
        Camera.main.GetComponent<FollowTarget>().Target = launchedWarble.transform;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.GameStateChange.Invoke(GameState.Maze);
    }

    public void KillWarble()
    {
        GameObject.Destroy(warble.gameObject);
        warble = null;
        GameManager.Instance.WarbleBreedController.ToggleButton();
        KillParticleEffect.Play();
    }

    public void DisableWarble(GameState state)
    {
        if(state == GameState.Transition && launchedWarble != null)
        {
            launchedWarble.enabled = false;
            launchedWarble = null;
            sticky = null;
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
