using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : PlayerComponent
{
    private Camera m_playerCamera;

    private UnitView m_selectedUnitView;
    private Transform m_selectedUnitTransform;

    [SerializeField] private LayerMask unitMask;

    protected override void Start()
    {
        base.Start();

        m_playerCamera = m_playerView.PlayerCamera;
    }

    // Update is called once per frame
    void Update()
    {
        SelectUnit();

        CancelSelection();
    }

    private void SelectUnit()
    {
        Ray ray = m_playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity,unitMask))
        {
            if(hit.transform != null)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    if(m_selectedUnitTransform == null || m_selectedUnitTransform != hit.transform)
                    {
                        SelectUnit(hit);
                    }
                }
            }
        }
    }

    private void SelectUnit(RaycastHit unit)
    {
        m_selectedUnitTransform = unit.transform;
        m_selectedUnitView = m_selectedUnitTransform.GetComponent<UnitView>();

        if(!m_selectedUnitView.CompareTag("EnemyUnit"))
        {
            m_selectedUnitView.UnitBehaviour.TurnAction(m_playerView, CancelSelection, CancelSelection);

            if (TurnManager.GetTurnPhase == TurnManager.TurnPhase.Movement || TurnManager.GetTurnPhase == TurnManager.TurnPhase.Shooting)
            {
                this.enabled = false;
            }
        }
    }

    private void RemoveUnit()
    {
        if(Input.GetMouseButtonDown(1))
        {
            CancelSelection();
        }
    }

    private void CancelSelection()
    {
        if (m_selectedUnitTransform != null)
        {
            m_selectedUnitTransform = null;
            m_selectedUnitView = null;

            this.enabled = true;
        }
    }
}
