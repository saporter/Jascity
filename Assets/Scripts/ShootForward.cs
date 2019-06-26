using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootForward : MonoBehaviour
{
    public float Speed = 15f;

    private float startLife;

    private void Start()
    {
        startLife = Time.time;
    }

    private void Update()
    {
        if (Time.time - startLife > 10f)
            Destroy(gameObject);

        transform.localPosition += Vector3.up * Speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var target = collision.gameObject.GetComponent<MoveWithPlayer>();
        if (target != null && target.Destructable)
            target.StopPlaying();
        Destroy(gameObject);
    }

}
