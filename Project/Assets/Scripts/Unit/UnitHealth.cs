using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : UnitComponent
{
    private float m_unitHealthPoints;

    protected override void Start()
    {
        base.Start();

        m_unitHealthPoints = m_unitView.UnitData.healthPoints;
    }

    [PunRPC]
    public void RemoveHealthPoints(int numberToRemove)
    {
        m_unitHealthPoints -= numberToRemove;

        if(m_unitHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
