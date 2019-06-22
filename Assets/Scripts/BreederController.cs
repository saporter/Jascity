using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreederController : MonoBehaviour
{
    public bool HasWarble { get { return hasWarble; } }
    public Genes WarbleGenes { get; set; }
    public GameObject GeneUI;
    public SpawnerController SpawnController;

    private bool hasWarble = false;
    private GeneUIController ui;
    private CanvasGroup cg;

    private void Awake()
    {
        ui = GeneUI.GetComponent<GeneUIController>();
        cg = GeneUI.GetComponent<CanvasGroup>();
    }

    private void OnTriggerEnter(Collider other)
    {
        cg.alpha = 1f;
        hasWarble = true;
        WarbleGenes = other.gameObject.GetComponent<Genes>();
        ui.ShowGenes(WarbleGenes.genes);
        SpawnController.WarbleEnterExit();
    }

    private void OnTriggerExit(Collider other)
    {
        cg.alpha = 0f;
        hasWarble = false;
        SpawnController.WarbleEnterExit();
    }
}
