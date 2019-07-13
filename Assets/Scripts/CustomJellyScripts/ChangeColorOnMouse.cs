using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnMouse : MonoBehaviour
{
    public Color RolloverColor;

    private SpriteRenderer r;
    private Color startColor;

    private void Awake()
    {
        r = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startColor = r.color;
    }

    private void OnMouseEnter()
    {
        r.color = RolloverColor;
    }

    private void OnMouseExit()
    {
        r.color = startColor;
    }
}
