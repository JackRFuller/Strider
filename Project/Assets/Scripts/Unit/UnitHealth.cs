using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitHealth : UnitComponent
{
    public Action UnitDied;

    private float m_unitHealthPoints;    

    protected override void Start()
    {
        base.Start();

        m_unitHealthPoints = m_unitView.UnitData.healthPoints;
    }

    public void RemoveHealthPoints(int numberToRemove)
    {
        m_unitHealthPoints -= numberToRemove;

        if(m_unitHealthPoints <= 0)
        {
            if (UnitDied != null)
                UnitDied.Invoke();

            Destroy(gameObject,5);
        }
    }
}
