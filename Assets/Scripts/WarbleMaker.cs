using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarbleMaker : MonoBehaviour
{
    public GameObject WarblePrefab;

    public void MakeNewRandomWarble()
    {
        GameObject.Instantiate(WarblePrefab, transform.position, transform.rotation, transform);
    }
}
