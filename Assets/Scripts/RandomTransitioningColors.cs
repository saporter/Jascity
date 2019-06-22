using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTransitioningColors : MonoBehaviour
{
    public float ChangeRate = 1f;
    public float[] TransitionTimeRange = new float[2] { 0.2f, 0.5f };

    private float[] rgb = new float[3];
    private float transitionTime = 0f;
    private int colorToModify;
    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();

        rgb[0] = Random.Range(0f, 1f);
        rgb[1] = Random.Range(0f, 1f);
        rgb[2] = Random.Range(0f, 1f);
    }

    private void Start()
    {
        rend.material.color = new Color(rgb[0], rgb[1], rgb[2]);
    }

    private void Update()
    {
        transitionTime -= Time.deltaTime;

        if(transitionTime < 0f)
        {
            transitionTime = Random.Range(TransitionTimeRange[0], TransitionTimeRange[1]);
            colorToModify = Random.Range(0, 3);
            ChangeRate = Random.Range(0, 2) == 0 ? ChangeRate : -ChangeRate;
        }

        rgb[colorToModify] += ChangeRate * Time.deltaTime;
        rgb[colorToModify] = rgb[colorToModify] > 1f ? 1f : rgb[colorToModify] < 0f ? 0f : rgb[colorToModify];

        rend.material.color = new Color(rgb[0], rgb[1], rgb[2]);
    }

}
