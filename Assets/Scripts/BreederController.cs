using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreederController : MonoBehaviour
{
    public GameObject GeneUI;
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
        ui.ShowGenes(other.gameObject.GetComponent<Genes>().genes);
    }

    private void OnTriggerExit(Collider other)
    {
        cg.alpha = 0f;
    }
}
