using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 60f);
        transform.position = Camera.main.ScreenToWorldPoint(point);
    }
}
