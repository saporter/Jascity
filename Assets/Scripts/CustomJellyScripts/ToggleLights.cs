using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLights : MonoBehaviour
{
    public bool LightsOn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.WorldLight.enabled = LightsOn;
    }
}
