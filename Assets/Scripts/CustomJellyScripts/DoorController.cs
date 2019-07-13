using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnMouseUpAsButton()
    {
        anim.SetTrigger("Open");
        GameManager.Instance.GameStateChange.Invoke(GameState.Transition);

        Vector3 pos = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        StartCoroutine(HelperTools.MoveCameraToPoint(pos, 1f));
        StartCoroutine(MoveToCaveIn(1f));
    }

    IEnumerator MoveToCaveIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        float travelTime = 4f;
        StartCoroutine(HelperTools.MoveCameraToPoint(GameManager.Instance.CavePosition.position, travelTime));
        yield return new WaitForSeconds(travelTime);
        GameManager.Instance.GameStateChange.Invoke(GameState.Cave);
    }

}
