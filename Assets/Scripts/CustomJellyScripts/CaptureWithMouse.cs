using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureWithMouse : MonoBehaviour
{
    private static bool AnythingWarping;
    private bool Warping { get { return _warping; } set { _warping = value; AnythingWarping = value; } }
    private bool _warping;

    public StickToSurface StickyComponent;
    public UnityJellySprite ParentJellySprite;
    ParticleSystem warpParticleSystem;  // attached to mouse and grabbed via GameManager
    public float PortalWarpSpeed = 1f;
    public float PortalMaxScale = 10f;

    private Transform warpParentTransform;
    private Vector3 startScale;
    private Vector3 startWarpScale;
    private float startGravityScale;
    private WarbleActions actions;
    private ClickAndDrag dragController;
    private ReturnToLab LabReturnServicer;

    private void Awake()
    {
        startScale = ParentJellySprite.transform.localScale;
        actions = ParentJellySprite.GetComponent<WarbleActions>();
        dragController = GetComponent<ClickAndDrag>();
        Warping = false;
    }

    private void Start()
    {
        warpParticleSystem = GameManager.Instance.WarpParticleSystem;
        warpParentTransform = warpParticleSystem.transform.parent;
        startWarpScale = warpParticleSystem.transform.localScale;
        startGravityScale = ParentJellySprite.m_GravityScale;
        LabReturnServicer = Camera.main.GetComponent<ReturnToLab>();
        GameManager.Instance.GameStateChange.AddListener(WaitForReturnToLab);
    }

    private void OnMouseDown()
    {
        if (!AnythingWarping)
        {
            StickyComponent.Unstick(0.1f);
            Warping = true;
            actions.PauseActions = true;
            ParentJellySprite.m_GravityScale = 0f;
            ParentJellySprite.InitMass(); // to update gravity
            //ParentJellySprite.
            StartCoroutine(GrowPortal(PortalWarpSpeed));
        }
    }

    private void OnMouseDrag()
    {
        StickyComponent.Unstick(0.1f);
    }

    private void OnMouseUp()
    {
        if (portalGrowing)
        {
            Warping = false;
            AnythingWarping = true;
            StartCoroutine(ReturnToNormal(PortalWarpSpeed));
        }
    }

    private bool portalGrowing = false;
    IEnumerator GrowPortal(float seconds)
    {
        portalGrowing = true;
        if (!warpParticleSystem.isPlaying) warpParticleSystem.Play();

        float end = Time.time + seconds;
        float scale = 0f;
        Vector3 max = new Vector3(PortalMaxScale, PortalMaxScale, PortalMaxScale);

        ParentJellySprite.SetPosition(warpParticleSystem.transform.position, false);
        yield return null;
        while (Time.time < end && Warping)
        {
            scale = 1f - (end - Time.time) / seconds;
            warpParticleSystem.transform.localScale = max * scale;
            ParentJellySprite.CentralPoint.Body2D.MovePosition(warpParticleSystem.transform.position);
            yield return null; //new WaitForSeconds(0.05f);
        }

        if(Warping)
            StartCoroutine(ShrinkEverything(seconds));

        portalGrowing = false;
    }

    IEnumerator ShrinkEverything(float seconds)
    {
        // Stop following mouse
        warpParticleSystem.transform.SetParent(null);
        

        // Shrink
        float end = Time.time + seconds;
        float scale = 0f;
        Vector3 max = warpParticleSystem.transform.localScale;

        ParentJellySprite.SetPosition(warpParticleSystem.transform.position, false);
        yield return null;
        while (Time.time < end && Warping)
        {
            scale = (end - Time.time) / seconds;
            warpParticleSystem.transform.localScale = max * scale;
            ParentJellySprite.transform.localScale = startScale * scale;
            ParentJellySprite.CentralPoint.Body2D.MovePosition(warpParticleSystem.transform.position);
            yield return null; 
        }

        if(Warping)
        {
            if(GameManager.Instance.WarbleCageManager.PlaceRigidbodyInCage(ParentJellySprite))
                StartCoroutine(LabReturnServicer.ReturnToTheLab());
            else
                StartCoroutine(ReturnToNormal(seconds));
        }
    }

    private void WaitForReturnToLab(GameState gameState)
    {
        if (!Warping)
            return;

        if (gameState == GameState.Lab)
        {
            StartCoroutine(ReturnToNormal(PortalWarpSpeed));
        }
    }

    private bool alreadyReturning = false;
    IEnumerator ReturnToNormal(float seconds)
    {
        // Only call once
        if (alreadyReturning)
            yield break;
        alreadyReturning = true;
        yield return null; // give other event listeners a chance to run, including those that might deactivate particle system

        if (!warpParticleSystem.isPlaying) { warpParticleSystem.Play(); }
        warpParticleSystem.transform.position = ParentJellySprite.CentralPoint.Body2D.position;

        // Return to normal
        Vector3 max = new Vector3(PortalMaxScale, PortalMaxScale, PortalMaxScale);
        float scale = 1f - warpParticleSystem.transform.localScale.x / max.x;
        float end = Time.time + seconds * (scale);
        while (Time.time < end)
        {
            scale = 1 - (end - Time.time) / seconds;
            ParentJellySprite.transform.localScale = startScale * scale;
            ParentJellySprite.CentralPoint.Body2D.MovePosition(warpParticleSystem.transform.position);
            warpParticleSystem.transform.localScale = max * scale;
            yield return null; //new WaitForFixedUpdate();
        }

        // alreadyReturning set to false in next coroutine
        actions.PauseActions = false;
        ParentJellySprite.m_GravityScale = startGravityScale;
        ParentJellySprite.InitMass();
        StartCoroutine(ReturnPortal(seconds));
    }

    IEnumerator ReturnPortal(float seconds)
    {
        // Return to start size
        float end = Time.time + seconds;
        float scale = 1f;
        Vector3 max = new Vector3(PortalMaxScale, PortalMaxScale, PortalMaxScale);
        while (Time.time < end)
        {
            scale = (end - Time.time) / seconds;
            warpParticleSystem.transform.localScale = max * scale;
            yield return null; //new WaitForSeconds(0.05f);
        }

        // Return particle system to mouse
        warpParticleSystem.transform.SetParent(warpParentTransform);
        warpParticleSystem.transform.localPosition = Vector3.zero;
        warpParticleSystem.transform.localScale = startWarpScale;
        if (GameManager.Instance.CurrentGameState == GameState.Lab)
        {
            warpParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // Reset
        Warping = false;
        alreadyReturning = false;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.GameStateChange.RemoveListener(WaitForReturnToLab);
    }
}
