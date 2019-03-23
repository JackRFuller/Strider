using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitComponent : MonoBehaviour
{
    protected UnitView m_unitView;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_unitView = GetComponent<UnitView>();        
    }

   
}
