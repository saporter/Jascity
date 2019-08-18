using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageManager : MonoBehaviour
{
    public Transform[] Cages;

    public bool PlaceRigidbodyInCage(UnityJellySprite specimen)
    {
        for(int i = 0; i < Cages.Length; ++i)
        {
            var controller = Cages[i].GetComponentInChildren<CageController>();
            if (controller.Warble == null)
            {
                specimen.SetPosition(Cages[i].position, true);
                return true;
            }
        }

        return false;
    }

    /***
     * Return Vector3.zero if no open positions... so don't put a cage at that position.
     * */
    public Vector3 NextOpenPosition()
    {
        for (int i = 0; i < Cages.Length; ++i)
        {
            var controller = Cages[i].GetComponentInChildren<CageController>();
            if (controller.Warble == null)
            {
                return Cages[i].position;
            }
        }

        return Vector3.zero;
    }
}
