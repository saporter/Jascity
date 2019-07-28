using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    public LayerMask KilledByContactWithLayers;

    List<IDefense> defenses = new List<IDefense>();

    private void Awake()
    {
        defenses = new List<IDefense>
        {
            GetComponent<ShockController>(),
            GetComponent<BurrowController>()
        };
    }

    public bool KillGameObject(GameObject attacker)
    {
        foreach(IDefense d in defenses)
        {
            if(d != null && d.DefendedAgainst(attacker))
            {
                return false;
            }
        }

        // TODO: Something cool here
        Destroy(gameObject);
        return true;
    }

    void OnJellyCollisionEnter2D(JellySprite.JellyCollision2D collision)
    {
        if (KilledByContactWithLayers == (KilledByContactWithLayers | (1 << collision.Collision2D.gameObject.layer)))
        {
            KillGameObject(collision.Collision2D.gameObject);
        }
    }
}
