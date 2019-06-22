using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchController : MonoBehaviour
{
    public GameObject RandomButton;
    public GameObject CountdownText;
    public GameManager gameManager;

    private CanvasGroup cgRandom;
    private CanvasGroup cgCountdown;
    private Coroutine countdownCoroutine;
    private Text countdownText;
    private bool keepFading = false;

    private void Awake()
    {
        cgRandom = RandomButton.GetComponent<CanvasGroup>();
        cgCountdown = CountdownText.GetComponent<CanvasGroup>();
        countdownText = CountdownText.GetComponent<Text>();
    }

    private void Start()
    {
        HelperTools.ToggleOff(cgCountdown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.Racing)
            return;


        HelperTools.ToggleOff(cgRandom);
        HelperTools.ToggleOn(cgCountdown);

        if(countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(Countdown(other.gameObject));
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameManager.Racing)
            return;

        if(countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
        keepFading = false;
        HelperTools.ToggleOn(cgRandom);
        HelperTools.ToggleOff(cgCountdown);
        
    }

    IEnumerator Countdown(GameObject warble)
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "";
        gameManager.SetupRace(warble);
        StartCoroutine(WaitForRaceEnd());
        yield return new WaitForSeconds(1.0f);
        countdownText.text = "Go!";
        gameManager.StartRace();
        yield return new WaitForSeconds(1.0f);
        keepFading = true;
        StartCoroutine(Fade());
        countdownCoroutine = null;
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(0.1f);
        while (cgCountdown.alpha > 0.0f && keepFading)
        {
            cgCountdown.alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
            
        }
        keepFading = false;
        HelperTools.ToggleOff(cgCountdown);
    }

    IEnumerator WaitForRaceEnd()
    {
        while(GameManager.Instance.Racing)
        {
            yield return new WaitForSeconds(0.1f);
        }
        HelperTools.ToggleOn(cgRandom);
    }
}
