using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genes : MonoBehaviour
{
    public Gene[] genes;

    LoadBehaviours load;
    GeneBehaviours controller;

    private void Awake()
    {
        load = GetComponent<LoadBehaviours>();
        controller = GetComponent<GeneBehaviours>();
    }

    private void Start()
    {
        genes = new Gene[load.Size];
        for(int i = 0; i < genes.Length; ++i)
        {
            genes[i] = new Gene {
                leftHalf = GeneBuilder.MakeAllele(load.leftBehaviours[i], load.leftDominance[i]),
                rightHalf = GeneBuilder.MakeAllele(load.rightBehaviours[i], load.rightDominance[i]) };
        }
    }

    public void ExpressGeneBehaviour(int geneIndex, Vector2 targetPosition)
    {
        if (geneIndex < 0 || geneIndex >= genes.Length)
        {
            Debug.LogError("gene index out of bounds: " + geneIndex + " for length " + genes.Length);
            return;
        }

        Gene gene = genes[geneIndex];

        if(gene.leftHalf.dominance > gene.rightHalf.dominance)
        {
            InvokeBehaviour(gene.leftHalf.behaviour, targetPosition);
        }
        else if (gene.leftHalf.dominance < gene.rightHalf.dominance)
        {
            InvokeBehaviour(gene.rightHalf.behaviour, targetPosition);
        }
        else
        {
            InvokeBehaviour(gene.leftHalf.behaviour, targetPosition);
            if(gene.leftHalf.behaviour != gene.rightHalf.behaviour)
            {
                InvokeBehaviour(gene.rightHalf.behaviour, targetPosition);
            }
        }
        
    }

    void InvokeBehaviour(Behaviour behaviour, Vector2 targetPosition)
    {
        switch (behaviour)
        {
            case Behaviour.shock:
                controller.Shock();
                break;
            case Behaviour.avoid:
                controller.Running(targetPosition, 1f);
                break;
            case Behaviour.follow:
                controller.Running(targetPosition, -1f);
                break;
            case Behaviour.jumpTowards:
                controller.JumpTowards(targetPosition);
                break;
        }
    }
}

class GeneBuilder
{
    static Dictionary<Behaviour, Color> alleleColors = new Dictionary<Behaviour, Color>()
    {
        {Behaviour.random, Color.white },
        {Behaviour.shock, Color.red },
        {Behaviour.avoid, Color.green },
        {Behaviour.follow, Color.blue },
        {Behaviour.jumpTowards, Color.yellow}
    };

    public static Allele MakeAllele(Behaviour behave, float dom)
    {
        return new Allele { behaviour = behave, dominance = dom, color = alleleColors[behave] };
    }
}

public struct Gene
{
    public Allele leftHalf;
    public Allele rightHalf;
}

public struct Allele
{
    public Behaviour behaviour;
    public float dominance;
    public Color color;
}

public enum Behaviour
{
    random,
    shock,
    avoid,
    follow,
    jumpTowards
}
