using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBehaviours : MonoBehaviour
{
    public int Size { get { return leftBehaviours.Length; } }
    public Behaviour[] leftBehaviours;
    public float[] leftDominance;
    public Behaviour[] rightBehaviours;
    public float[] rightDominance;

    private void Awake()
    {
        if (leftBehaviours.Length == leftDominance.Length)
            if (leftDominance.Length == rightBehaviours.Length)
                if (rightBehaviours.Length == rightDominance.Length)
                    return;
        Debug.LogError("Unequal Load Behaviour elements ");
    }
}
