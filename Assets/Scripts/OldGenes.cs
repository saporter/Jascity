using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGenes : MonoBehaviour
{
    private const int number_of_behaviors = 3;
    private const float color_step = 1f / (float)number_of_behaviors;

    public AGene[] genes;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        Mate(null, null);
    }
    
    public void Mate(AGene[] genesMale, AGene [] genesFemale)
    {
        genes = new AGene[number_of_behaviors];

        if (genesMale == null || genesFemale == null)
        {
            for (int i = 0; i < genes.Length; ++i)
            {
                genes[i] = new AGene {oneHalf = (AnAllele)Random.Range(0, 6), otherHalf = (AnAllele)Random.Range(0, 6) };
            }
        }
        else
        {
            for (int i = 0; i < genes.Length; ++i)
            {
                genes[i] = new AGene {
                    oneHalf = Random.Range(0, 2) == 0 ? genesMale[i].oneHalf : genesMale[i].otherHalf,
                    otherHalf = Random.Range(0, 2) == 0 ? genesFemale[i].oneHalf : genesFemale[i].otherHalf
                };
            }
        }

        rend.material.color = UpdateColor();
    }

    private Color UpdateColor()
    {
        float r = 0f;
        float g = 0f;
        float b = 0f;

        for(int i = 0; i < number_of_behaviors; ++i)
        {
            switch (genes[i].oneHalf)
            {
                case AnAllele.shoot:
                    r += color_step;
                    break;
                case AnAllele.avoid:
                    g += color_step;
                    break;
                case AnAllele.tackle:
                    b += color_step;
                    break;
                case AnAllele.shootDominant:
                    r += color_step * 2;
                    break;
                case AnAllele.avoidDominant:
                    g += color_step * 2;
                    break;
                case AnAllele.tackleDominant:
                    b += color_step * 2;
                    break;
            }
            switch (genes[i].otherHalf)
            {
                case AnAllele.shoot:
                    r += color_step;
                    break;
                case AnAllele.avoid:
                    g += color_step;
                    break;
                case AnAllele.tackle:
                    b += color_step;
                    break;
                case AnAllele.shootDominant:
                    r += color_step * 2;
                    break;
                case AnAllele.avoidDominant:
                    g += color_step * 2;
                    break;
                case AnAllele.tackleDominant:
                    b += color_step * 2;
                    break;
            }
        }

        r = r > 1f ? 1f : r < 0 ? 0 : r;
        g = g > 1f ? 1f : g < 0 ? 0 : g;
        b = b > 1f ? 1f : b < 0 ? 0 : b;
        return new Color(r, g, b);
    }

}

/**
 * A gene consists of two alleles.  Each Gene in this game will determine behavior in one of 9 (or so) instances of the environment.
 * 
 */
public struct AGene
{
    public AnAllele oneHalf;
    public AnAllele otherHalf;
}

/**
 * Allele is one half of a gene.  The dominant allele determines characteristics.  If both are dominant or recessive then both contribute to characteristics.
 * This enum holds four possible behaviors.
 * */
public enum AnAllele
{
    // Recessive alleles
    shoot,
    avoid,
    tackle,
    
    // Dominant alleles
    shootDominant,
    avoidDominant,
    tackleDominant
}
