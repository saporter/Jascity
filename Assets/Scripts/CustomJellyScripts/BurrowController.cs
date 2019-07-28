using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowController : MonoBehaviour, IDefense
{
    public float UnhideDelay = 0.2f;
    public ParticleSystem DustParticle;
    public SpriteRenderer DirtMound;
    public bool Burried { get { return hidden || !canHide; } }


    bool ToldToHide { get { return Time.time - lastHide < UnhideDelay; } }

    private UnityJellySprite jelly;
    private WarbleActions actions;
    private StickToSurface sticky;
    private float lastHide;
    private float startStiffness;
    private float startRadius = -1f;
    private bool hidden = false;
    private bool canHide = true;

    public bool DefendedAgainst(GameObject attacker)
    {
        return hidden || !canHide;
    }

    private void Awake()
    {
        jelly = GetComponent<UnityJellySprite>();
        actions = GetComponent<WarbleActions>();
        sticky = GetComponent<StickToSurface>();
    }

    private void Start()
    {
        startStiffness = jelly.m_Stiffness;
    }

    private void Update()
    {
        if (Time.time < UnhideDelay)
            return;

        if (!ToldToHide && hidden)
            Unhide();
    }

    private void FixedUpdate()
    {
        if (hidden || !canHide)
        {
            jelly.AddForce(-actions.UpVector * actions.JumpForce);
        }
    }

    public void Hide()
    {
        lastHide = Time.time;
        HideJelly();
    }

    void HideJelly()
    {
        if (!hidden && canHide)
        {
            startRadius = jelly.CentralPoint.Collider2D.radius;
            jelly.CentralPoint.Collider2D.radius = 0.1f;
            foreach (JellySprite.ReferencePoint r in jelly.ReferencePoints) { r.Collider2D.radius = 0.1f; }
            jelly.UpdateJoints();
            DustParticle.Play();
            DirtMound.enabled = true;
            DustParticle.transform.rotation = Quaternion.LookRotation(Vector3.forward, actions.UpVector);
            DirtMound.transform.rotation = Quaternion.LookRotation(Vector3.forward, actions.UpVector);
            StartCoroutine(DoubleCheckRotations(0.1f));
        }

        hidden = true;
    }

    void Unhide()
    {
        if (startRadius < 0f)
            return;

        StartCoroutine(ReturnToSurface());
        hidden = false;
    }

    IEnumerator ReturnToSurface()
    {
        canHide = false;
        DustParticle.Play();
        yield return new WaitForSeconds(0.05f);
        DirtMound.enabled = false;
        yield return new WaitForSeconds(0.3f);
        jelly.m_Stiffness = startStiffness;
        jelly.CentralPoint.Collider2D.radius = startRadius;
        foreach (JellySprite.ReferencePoint r in jelly.ReferencePoints) { r.Collider2D.radius = startRadius; }
        //sticky.Unstick(0.0f);
        jelly.UpdateJoints();
        canHide = true;
    }

    IEnumerator DoubleCheckRotations(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DirtMound.transform.rotation = Quaternion.LookRotation(Vector3.forward, actions.UpVector);
        DustParticle.transform.rotation = Quaternion.LookRotation(Vector3.forward, actions.UpVector);
    }

}
