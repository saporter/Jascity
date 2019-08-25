using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToLab : MonoBehaviour
{
    public Text CountdownText;
    public Image Shade;
    public float FadingTime = 1f;

    private CanvasGroup textCG;
    private CanvasGroup shadeCG;

    private void Start()
    {
        GameManager.Instance.GameStateChange.AddListener(StateChange);
        textCG = CountdownText.GetComponent<CanvasGroup>();
        shadeCG = Shade.GetComponent<CanvasGroup>();
        HelperTools.ToggleOff(textCG);
    }

    void StateChange(GameState state)
    {
        switch(state)
        {
            case GameState.Cave:
            case GameState.Maze:
                StartCoroutine(ListenForReturn());
                break;
            case GameState.Lab:
            case GameState.Transition:
                listening = false;
                break;
        }
    }

    private bool listening = false;
    IEnumerator ListenForReturn()
    {
        float countdown = 3;
        float time = 0f;
        listening = true;

        while (listening)
        {
            var exit = Input.GetAxis("Return");
            if (exit != 0)
            {
                HelperTools.ToggleOn(textCG);
                countdown = Mathf.Ceil(3f - 2f*(Time.time - time));
                CountdownText.text = "" + countdown;
                if (countdown <= 0)
                {
                    HelperTools.ToggleOff(textCG);
                    yield return ReturnToTheLab();
                }
            }
            else
            {
                HelperTools.ToggleOff(textCG);
                time = Time.time;
                countdown = 3;
            }
            yield return null;
        }
    }

    public IEnumerator ReturnToTheLab()
    {
        listening = false;
        GameManager.Instance.GameStateChange.Invoke(GameState.Transition);
        StartCoroutine(FadeToLabIn(FadingTime));
        yield return new WaitForSeconds(FadingTime*2f + 0.05f);
        GameManager.Instance.GameStateChange.Invoke(GameState.Lab);
    }

    IEnumerator FadeToLabIn(float seconds)
    {
        float end = Time.time + seconds;
        while(end > Time.time)
        {
            yield return new WaitForSeconds(0.05f);
            shadeCG.alpha = 1 - (end - Time.time) / seconds;
        }

        shadeCG.alpha = 1f;
        yield return new WaitForSeconds(0.05f);
        Camera.main.transform.position = GameManager.Instance.LabPosition.position;

        end = Time.time + seconds;
        while (end > Time.time)
        {
            yield return new WaitForSeconds(0.05f);
            shadeCG.alpha = (end - Time.time) / seconds;
        }

        shadeCG.alpha = 0f;
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.GameStateChange.RemoveListener(StateChange);
        }
    }
}
