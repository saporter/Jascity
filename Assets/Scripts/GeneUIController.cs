using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneUIController : MonoBehaviour
{
    public Sprite Recessive;
    public Sprite Dominant;
    private CanvasGroup cg;

    Image[] alleles;
    private void Awake()
    {
        alleles = GetComponentsInChildren<Image>();
        cg = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        cg.alpha = 0.0f;
    }

    public void ShowGenes(AGene[] genes)
    {
        for(int i = 0; i < genes.Length; ++i)
        {
            ColorSprite oneHalf = GetColorAndSprite(genes[i].oneHalf);
            ColorSprite otherHalf = GetColorAndSprite(genes[i].otherHalf);
            alleles[2 * i].color = oneHalf.color;
            alleles[2 * i].sprite = oneHalf.sprite;
            alleles[2 * i + 1].color = otherHalf.color;
            alleles[2 * i + 1].sprite = otherHalf.sprite;
        }
    }

    ColorSprite GetColorAndSprite(AnAllele allele)
    {
        switch (allele)
        {
            case AnAllele.shootDominant:
                return new ColorSprite { color = new Color(255f, 0f, 0f), sprite = Dominant };
            case AnAllele.shoot:
                return new ColorSprite { color = new Color(128f, 0f, 0f), sprite = Recessive };
            case AnAllele.avoidDominant:
                return new ColorSprite { color = new Color(0f, 255f, 0f), sprite = Dominant };
            case AnAllele.avoid:
                return new ColorSprite { color = new Color(0f, 128f, 0f), sprite = Recessive };
            case AnAllele.tackleDominant:
                return new ColorSprite { color = new Color(0f, 0f, 255f), sprite = Dominant };
            case AnAllele.tackle:
                return new ColorSprite { color = new Color(0f, 0f, 128f), sprite = Recessive };
        }

        // Should never happen
        Debug.LogWarning("Gene with no behavior created");
        return new ColorSprite { color = new Color(0f, 0f, 0f), sprite = Recessive };
    }
}

struct ColorSprite
{
    public Color color;
    public Sprite sprite;
}
