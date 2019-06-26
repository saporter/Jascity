using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject TextUI;
    public float StartCountdownFrom = 30f;
    public float AdditionalTime = 0f;  // Rewards will add to this
    public float TimeLeft { get; private set; }

    private float startTime;
    private bool counting = false;
    private CanvasGroup cg;
    private Text text;

    private void Awake()
    {
        cg = TextUI.GetComponent<CanvasGroup>();
        text = TextUI.GetComponent<Text>();
        TimeLeft = StartCountdownFrom;
    }

    private void Start()
    {
        counting = false;
        HelperTools.ToggleOff(cg);
    }

    public void StartTimer()
    {
        TimeLeft = StartCountdownFrom;
        AdditionalTime = 0f;
        startTime = Time.time;
        counting = true;
        HelperTools.ToggleOn(cg);
        StartCoroutine(Counting());
    }

    public void StopTimer()
    {
        counting = false;
        HelperTools.ToggleOff(cg);
    }

    public void PauseTimer()
    {
        counting = false;
    }

    IEnumerator Counting()
    {
        float minutes;
        float seconds;
        while(counting && TimeLeft > 0f)
        {
            TimeLeft = StartCountdownFrom - (UnityEngine.Time.time - startTime) + AdditionalTime;
            minutes = Mathf.Floor((TimeLeft) / 60f);
            seconds = (TimeLeft) % 60;

            text.text = 
                (minutes < 10 ? "0" + minutes.ToString() : minutes.ToString()) + 
                ":" + 
                (seconds < 10 ? "0" + Mathf.Floor(seconds).ToString() : Mathf.Floor(seconds).ToString());

            if(TimeLeft > 5f && text.color != Color.white)
            {
                text.color = Color.white;
            }
            else if(TimeLeft < 5f && text.color != Color.red)
            {
                text.color = Color.red;
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

}
