using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarblesReactToThis : MonoBehaviour
{
    public int GeneBehaviourIndex = 0;  // Which gene determines behaviour when encountering this object

    List<Genes> warbles;
    private void Awake()
    {
        warbles = new List<Genes>(10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var w = collision.GetComponent<Genes>();
        if (w != null && !warbles.Contains(w))
        {
            warbles.Add(w);
            
        }
    }

    private void FixedUpdate()
    {
        foreach (Genes controller in warbles)
        {
            controller.ExpressGeneBehaviour(GeneBehaviourIndex, transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var w = collision.GetComponent<Genes>();
        if (w != null && warbles.Contains(w))
        {
            warbles.Remove(w);
        }
    }
}
