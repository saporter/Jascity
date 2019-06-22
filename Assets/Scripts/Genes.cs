using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genes : MonoBehaviour
{
    private const int number_of_behaviors = 9;
    private const float color_step = 255 / number_of_behaviors;
    public Gene[] genes;

    private void Awake()
    {
        Mate(null, null);
        
    }
    
    public void Mate(Gene[] genesMale, Gene [] genesFemale)
    {
        if(genesMale == null || genesFemale == null)
        {
            

            genes = new Gene[number_of_behaviors];

            for (int i = 0; i < genes.Length; ++i)
            {
                genes[i] = new Gene {oneHalf = (Allele)Random.Range(0, 9), otherHalf = (Allele)Random.Range(0, 9) };
            }
        }
        else
        {
            //TODO
        }

        GetComponent<Renderer>().material.color = UpdateColor();
        
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
                case Allele.left:
                    r += color_step;
                    break;
                case Allele.right:
                    g += color_step;
                    break;
                case Allele.up:
                    r -= color_step;
                    g -= color_step;
                    b -= color_step;
                    break;
                case Allele.down:
                    b += color_step;
                    break;
                case Allele.leftDom:
                    r += color_step * 2;
                    break;
                case Allele.rightDom:
                    g += color_step * 2;
                    break;
                case Allele.upDom:
                    r -= color_step * 2;
                    g -= color_step * 2;
                    b -= color_step * 2;
                    break;
                case Allele.downDom:
                    b += color_step * 2;
                    break;
            }
            switch (genes[i].otherHalf)
            {
                case Allele.left:
                    r += color_step;
                    break;
                case Allele.right:
                    g += color_step;
                    break;
                case Allele.up:
                    r -= color_step;
                    g -= color_step;
                    b -= color_step;
                    break;
                case Allele.down:
                    b += color_step;
                    break;
                case Allele.leftDom:
                    r += color_step * 2;
                    break;
                case Allele.rightDom:
                    g += color_step * 2;
                    break;
                case Allele.upDom:
                    r -= color_step * 2;
                    g -= color_step * 2;
                    b -= color_step * 2;
                    break;
                case Allele.downDom:
                    b += color_step * 2;
                    break;
            }
        }

        r = r > 255 ? 255 : r < 0 ? 0 : r;
        g = g > 255 ? 255 : g < 0 ? 0 : g;
        b = r > 255 ? 255 : b < 0 ? 0 : b;
        return new Color(r, g, b);
    }

}

/**
 * A gene consists of two alleles.  Each Gene in this game will determine behavior in one of 9 (or so) instances of the environment.
 * 
 */
public struct Gene
{
    public Allele oneHalf;
    public Allele otherHalf;
}

/**
 * Allele is one half of a gene.  The dominant allele determines characteristics.  If both are dominant or recessive then both contribute to characteristics.
 * This enum holds four possible behaviors.
 * */
public enum Allele
{
    // Recessive alleles
    left,
    right,
    up,
    down,
    
    // Dominant alleles
    leftDom,
    rightDom,
    upDom,
    downDom
}
