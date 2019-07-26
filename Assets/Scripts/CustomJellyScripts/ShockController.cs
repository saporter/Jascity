using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockController : MonoBehaviour, IDefense
{
    public ParticleSystem ChargingLightning;
    public ParticleSystem ShockLightning;
    public float ShockRestTime = 0.2f;
    public float ChargeRefreshRate = 0.2f;
    public LayerMask ShockLayersOnContact;

    public bool Invincible { get { return Charging; } }
    bool Charging { get { return Time.time - lastCharge < ChargeRefreshRate; } }
    bool ShockReady { get { return Time.time - lastShockTime > ShockRestTime; } }

    private float lastCharge;
    private float lastShockTime;
    

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Time.time < ChargeRefreshRate)
            return;

        if(!Charging)
        {
            ChargingLightning.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            return;
        }

        if(!ChargingLightning.isPlaying && ShockReady)
            ChargingLightning.Play(true);
    }
    
    public void Charge()
    {
        if(ShockReady)
            lastCharge = Time.time;
    }

    void OnJellyCollisionEnter2D(JellySprite.JellyCollision2D collision)
    {
        if (ShockLayersOnContact == (ShockLayersOnContact | (1 << collision.Collision2D.gameObject.layer)))
        {
            if (Charging && ShockReady)
            {
                ChargingLightning.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ShockLightning.Play(true);
                
                var jelly = collision.Collision2D.gameObject.GetComponent<JellySpriteReferencePoint>();
                GameObject toDestory = jelly != null ? jelly.ParentJellySprite.gameObject : collision.Collision2D.gameObject;
                var death = toDestory.GetComponent<DeathController>();
                if (death)
                {
                    death.KillGameObject();
                }

                lastShockTime = Time.time;
            }
        }
    }
}
