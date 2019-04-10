using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : UnitComponent
{
    [SerializeField]
    private Material m_enemyBaseMaterial;
    [SerializeField]
    private MeshRenderer m_unitBase;

    protected override void Start()
    {
        base.Start();

        //SetBaseColour();
    }

    private void SetBaseColour()
    {
        if (!m_unitView.PhotonView.isMine)
            m_unitBase.material = m_enemyBaseMaterial;
    }

    public void SetBaseColourRed()
    {
        m_unitBase.material = m_enemyBaseMaterial;
    }
}
