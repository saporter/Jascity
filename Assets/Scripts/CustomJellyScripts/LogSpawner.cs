using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    public GameObject LogPrefab;
    public float Frequency = 3f;

    private float lastLog = 0f;
    private void Update()
    {
        if(Time.time - lastLog > Frequency)
        {
            GameObject.Instantiate(LogPrefab, transform.position, Quaternion.identity);
            lastLog = Time.time;
        }
    }
}
