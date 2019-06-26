using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerController : MonoBehaviour
{
    public GameObject WarbleMaker;
    public GameObject WarblePrefab;
    public GameObject MateButton;
    public BreederController Left;
    public BreederController Right;

    private CanvasGroup cg;

    private void Awake()
    {
        cg = MateButton.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        cg.alpha = 0.0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void WarbleEnterExit()
    {
        if(Left.HasWarble && Right.HasWarble)
        {
            cg.alpha = 1.0f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0.0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void SpawnWarble()
    {
        Genes newWarble = GameObject.Instantiate<GameObject>(WarblePrefab).GetComponent<Genes>();
        newWarble.transform.position = transform.position;
        newWarble.transform.SetParent(WarbleMaker.transform);
        newWarble.Mate(Left.WarbleGenes.genes, Right.WarbleGenes.genes);
    }
}
