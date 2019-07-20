using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageManager : MonoBehaviour
{
    public Transform[] Cages;
    UnityJellySprite[] specimens;

    private void Awake()
    {
        specimens = new UnityJellySprite[Cages.Length];
    }

    public bool PlaceRigidbodyInCage(UnityJellySprite specimen)
    {
        for(int i = 0; i < specimens.Length; ++i)
        {
            if (specimens[i] == null)
            {
                specimen.SetPosition(Cages[i].position, true);
                specimens[i] = specimen;
                return true;
            }
        }

        return false;
    }
}
