using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAIBehaviour : UnitComponent
{
    private Action<Action> m_unitTurn;
    private Action m_unitTurnFinished;

    private UnitView m_primaryTargetUnit;

    public UnitView PrimaryTargetUnit { get { return m_primaryTargetUnit; } }
    
    protected override void Start()
    {
        base.Start();

        SetTurnAction();
        AssignTarget();
    }

    private void AssignTarget()
    {
        m_primaryTargetUnit = GameManager.Instance.UnitAIManager.GetTarget();
    }

    private void SetTurnAction()
    {
        switch (TurnManager.GetTurnPhase)
        {
            case TurnManager.TurnPhase.Movement:
                m_unitTurn = m_unitView.UnitAIMoveAction.DoAction;
                break;
            case TurnManager.TurnPhase.Shooting:
                m_unitTurn = m_unitView.UnitAIShootingAction.DoAction;
                break;
        }
    }

    public void StartTurnAction(Action actionFinished)
    {
        if(m_unitTurn != null)
            m_unitTurn.Invoke(actionFinished);
    }
}
