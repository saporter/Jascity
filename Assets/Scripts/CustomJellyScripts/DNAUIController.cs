using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAUIController : MonoBehaviour
{
    public GameObject GeneUIPrefab;

    float buffer;

    public void Start()
    {
        buffer = transform.GetChild(0).GetComponent<GeneUIController>().Height;
        DeleteCurrentDisplay();
    }

    public void WarbleAdded(Gene[] genes)
    {
        for(int i = 0; i < genes.Length; ++i)
        {
            var ui = Instantiate(GeneUIPrefab, transform).GetComponent<GeneUIController>();
            ui.SetGene(genes[i]);
            ui.transform.position += new Vector3(0f, buffer * (genes.Length - 1 - i), 0f);
        }
    }

    public void DeleteCurrentDisplay()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
