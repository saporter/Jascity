using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreederController : MonoBehaviour
{
    public GameObject WarblePrefab;
    public CanvasGroup Button_CG;

    private void Start()
    {
        HelperTools.ToggleOff(Button_CG);
    }

    public void ToggleButtonOn()
    {
        transform.position = GameManager.Instance.WarbleCageManager.NextOpenPosition();
        if (transform.position != Vector3.zero)
        {
            HelperTools.ToggleOn(Button_CG);
            return;
        }
        ToggleButtonOff();
    }

    public void ToggleButtonOff()
    {
        HelperTools.ToggleOff(Button_CG);
    }

    public void BreedTwoActiveWarbles()
    {
        Genes[] parents = GetParentGenes();
        Gene[] child = MakeChildGenes(parents);

        GameObject warble = GameObject.Instantiate(WarblePrefab);
        warble.GetComponent<Genes>().SetGenes(child);
        warble.transform.position = transform.position;

        StartCoroutine(UpdateButtonIn(0.1f));
    }

    private Genes[] GetParentGenes()
    {
        Genes[] warbles = new Genes[2];

        foreach (SwitchController toggle in SwitchController.AllSwitches)
        {
            if (toggle.On)
            {
                if (warbles[0] == null)
                {
                    warbles[0] = toggle.Cage.Warble.GetComponent<Genes>();
                }
                else if (warbles[1] == null)
                {
                    warbles[1] = toggle.Cage.Warble.GetComponent<Genes>();
                    return warbles; // should always return here
                }
            }
        }

        Debug.LogError("Two parent genes not found: " + warbles);
        return warbles; // just in case
    }

    private Gene[] MakeChildGenes(Genes[] parents)
    {
        int length = parents[0].GeneSequence.Length;
        Gene[] child = new Gene[length];

        for (int i = 0; i < length; ++i)
        {
            child[i] = new Gene
            {
                leftHalf = Random.Range(0, 2) == 0 ? parents[0].GeneSequence[i].leftHalf : parents[0].GeneSequence[i].rightHalf,
                rightHalf = Random.Range(0, 2) == 0 ? parents[1].GeneSequence[i].leftHalf : parents[1].GeneSequence[i].rightHalf
            };
        }

        return child;
    }

    private IEnumerator UpdateButtonIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ToggleButtonOn();
    }
}
