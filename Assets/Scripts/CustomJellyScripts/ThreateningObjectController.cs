using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreateningObjectController : MonoBehaviour
{
    List<NPCController> NPCs;
    private void Awake()
    {
        Debug.Log("This should really invoke a gene behavior");
        NPCs = new List<NPCController>(10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var npc = collision.GetComponent<NPCController>();
        if (npc != null && !NPCs.Contains(npc))
        {
            NPCs.Add(npc);
            Debug.Log("Adding: " + npc.name);
        }
    }

    private void FixedUpdate()
    {
        foreach (NPCController controller in NPCs)
        {
            controller.Running(transform.position, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var npc = collision.GetComponent<NPCController>();
        if (npc != null && NPCs.Contains(npc))
        {
            NPCs.Remove(npc);
            Debug.Log("Removing: " + npc.name);
        }
    }
}
