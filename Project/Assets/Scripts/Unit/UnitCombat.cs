using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    private bool isInCombat = false;
    public bool IsInCombat { get { return isInCombat; } }

    public void SetToInCombat()
    {
        isInCombat = true;
        Debug.Log("IsInCombat", gameObject);
    }
}
