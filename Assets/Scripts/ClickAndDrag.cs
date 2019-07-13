using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    Vector3 screenPoint;
    Vector3 scanPos;
    Vector3 offset;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    private void OnMouseDown()
    {
        if (OldGM.Instance.Racing)
            return;

        screenPoint = Camera.main.WorldToScreenPoint(scanPos);
        scanPos = transform.position;
        offset = scanPos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }
    private void OnMouseDrag()
    {
        if (OldGM.Instance.Racing)
            return;

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        rb.MovePosition(curPosition);
    }

    private void OnMouseUp()
    {
        if (OldGM.Instance.Racing)
            return;

        scanPos = transform.position;
        rb.velocity = Vector3.zero;
    }
}
