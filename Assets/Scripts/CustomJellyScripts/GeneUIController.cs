using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneUIController : MonoBehaviour
{
    public SpriteRenderer DominantLeft;
    public SpriteRenderer DominantRight;
    public SpriteRenderer RecessiveLeft;
    public SpriteRenderer RecessiveRight;

    public float Height { get { return DominantLeft.bounds.size.y; } }

    public void SetGene(Gene gene)
    {
        SpriteRenderer left = GetLeft(gene.leftHalf.dominance);
        SpriteRenderer right = GetRight(gene.rightHalf.dominance);
        left.color = gene.leftHalf.color;
        right.color = gene.rightHalf.color;
    }

    SpriteRenderer GetLeft(float dominance)
    {
        if (dominance == 1)
        {
            DominantLeft.enabled = true;
            RecessiveLeft.enabled = false;
            return DominantLeft;
        }
        else
        {
            DominantLeft.enabled = false;
            RecessiveLeft.enabled = true;
            return RecessiveLeft;
        }
    }

    SpriteRenderer GetRight(float dominance)
    {
        if (dominance == 1)
        {
            DominantRight.enabled = true;
            RecessiveRight.enabled = false;
            return DominantRight;
        }
        else
        {
            DominantRight.enabled = false;
            RecessiveRight.enabled = true;
            return RecessiveRight;
        }
    }
}
