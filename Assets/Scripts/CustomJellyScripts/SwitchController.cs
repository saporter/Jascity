﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public static List<SwitchController> AllSwitches;
    public static int SwitchesOnCount { get { return numberSwitchedOn; } }

    private static int numberSwitchedOn = 0;
    private static SwitchController lastSwitch;

    public string InputAxis;
    public Color OffColor = Color.gray;
    public CageController Cage;
    public bool On { get { return turnedOn; } }

    Color OnColor;
    bool turnedOn = true;
    SpriteRenderer r;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        r = GetComponent<SpriteRenderer>();
        OnColor = r.color;

        if (AllSwitches == null)
            AllSwitches = new List<SwitchController>(4);
        AllSwitches.Add(this);
    }

    private void OnMouseUpAsButton()
    {
        Toggle();
    }

    private void Update()
    {
        if (Input.GetButtonUp(InputAxis))
        {
            Toggle();
        };
    }

    public void Toggle()
    {
        if (!turnedOn)
            return;

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Off"))
        {
            numberSwitchedOn++;
        }
        else
        {
            numberSwitchedOn--;
        }
        anim.SetTrigger("Toggle");

        if(numberSwitchedOn > 2 && lastSwitch != null)
        {
            lastSwitch.Toggle();
        }

        GameManager.Instance.WarbleBreedController.ToggleButton();

        lastSwitch = this;
    }

    /***
     * Like SetActive() but custom
     * 
     * */
    public void Enable(bool enable)
    {
        if (enable)
        {
            r.color = OnColor;
        }
        else
        {
            r.color = OffColor;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("On"))
            {
                Toggle();
            }
        }

        turnedOn = enable;
        GetComponent<BoxCollider>().enabled = enable;
    }
}
