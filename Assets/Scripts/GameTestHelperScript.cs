using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTestHelperScript : MonoBehaviour
{
    CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }
    void Update()
    {
        var input = Input.GetAxis("Submit");
        if(input != 0f)
        {
            HelperTools.ToggleOn(cg);
        }
    }
}
