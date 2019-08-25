using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreederController : MonoBehaviour
{
    public GameObject WarblePrefab;
    public CanvasGroup Button_CG;

    private CageController currentCage;
    private CageController lastUsedCage;
    private IEnumerator breedCoroutine;
    private IEnumerator killCorountine;

    private void Start()
    {
        GameManager.Instance.GameStateChange.AddListener(HotkeyGameStateListener);
        HelperTools.ToggleOff(Button_CG);
    }

    public void ToggleButton()
    {
        if (SwitchController.SwitchesOnCount == 2)
        {
            if(killCorountine == null)
            {
                killCorountine = ListenForKill();
                StartCoroutine(killCorountine);
            }
            ToggleButtonOn();
        }
        else
        {
            StopKillCoroutine();
            ToggleButtonOff();
        }
    }

    private void StopKillCoroutine()
    {
        lastUsedCage = null;
        if (killCorountine != null)
        {
            StopCoroutine(killCorountine);
            killCorountine = null;
        }
    }

    private void StopBreedCoroutine()
    {
        if (breedCoroutine != null)
        {
            StopCoroutine(breedCoroutine);
            breedCoroutine = null;
        }
    }

    private void ToggleButtonOn()
    {
        CageController cage = GameManager.Instance.WarbleCageManager.NextOpenCage();
        if (cage != null)
        {
            transform.position = cage.transform.position;
            currentCage = cage;
            HelperTools.ToggleOn(Button_CG);
            
            if (breedCoroutine == null)
            {
                breedCoroutine = ListenForBreed();
                StartCoroutine(breedCoroutine);
            }
            return;
        }
        ToggleButtonOff();
    }

    private void ToggleButtonOff()
    {
        currentCage = null;
        StopBreedCoroutine();
        HelperTools.ToggleOff(Button_CG);
    }

    IEnumerator ListenForBreed()
    {
        while(true)
        {
            if (Input.GetButtonUp("Breed"))
            {
                BreedTwoActiveWarbles();
            }
            yield return null;
        }
    }

    IEnumerator ListenForKill()
    {
        while(true)
        {
            if (Input.GetButtonUp("Kill"))
            {
                if (lastUsedCage != null && lastUsedCage.Warble != null)
                {
                    lastUsedCage.KillWarble();
                    lastUsedCage = null;
                    ToggleButton();
                }
            }
            yield return null;
        }
    }

    public void BreedTwoActiveWarbles()
    {
        Genes[] parents = GetParentGenes();
        Gene[] child = MakeChildGenes(parents);
        lastUsedCage = currentCage;

        GameObject warble = GameObject.Instantiate(WarblePrefab);
        warble.GetComponent<Genes>().SetGenes(child);
        warble.transform.position = transform.position;

        // Turning off or moving button will be handled by cage controller 
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

    private void HotkeyGameStateListener(GameState state)
    {
        switch(state)
        {
            case GameState.Lab:
                ToggleButton();
                break;
            default:
                StopKillCoroutine();
                StopBreedCoroutine();
                break;
        }
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.GameStateChange.RemoveListener(HotkeyGameStateListener);
        }
    }

}
