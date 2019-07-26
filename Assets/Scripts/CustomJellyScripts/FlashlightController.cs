using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Warble"))
        {
            GameManager.Instance.HasSpotLight = true;
            GameObject.Destroy(gameObject);
        }
    }
}
