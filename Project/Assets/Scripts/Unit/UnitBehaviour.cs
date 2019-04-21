using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitBehaviour : UnitComponent
{
    private Action<Action,Action> m_unitTurn;
    private Action m_turnCancelledCallback;
    private Action m_turnEnactedCallback;

    private bool m_hasActedThisTurn = false;

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.TurnManager.TurnPhaseUpdated += SetTurnAction;

        SetTurnAction();
    }

    private void SetTurnAction()
    {
        m_hasActedThisTurn = false;

        switch (TurnManager.GetTurnPhase)
        {
            case TurnManager.TurnPhase.Movement:
                m_unitTurn = m_unitView.UnitMovement.StartUnitMovement;
                break;
            case TurnManager.TurnPhase.Shooting:
                m_unitTurn = m_unitView.UnitShooting.StartUnitShooting;
                break;
        }
    }

    public void TurnAction(PlayerView playerView, Action turnCancelledCallback, Action turnCompleteCallback)
    {
        if (m_hasActedThisTurn)
            return;

        m_turnCancelledCallback = turnCancelledCallback;
        m_turnEnactedCallback = turnCompleteCallback;

        m_unitView.SetPlayerView(playerView);

        if (m_unitTurn != null)
            m_unitTurn.Invoke(TurnEnacted, TurnCancelled);
    }

    private void TurnCancelled()
    {
        if (m_turnCancelledCallback != null)
            m_turnCancelledCallback.Invoke();
    }

    private void TurnEnacted()
    {
        m_hasActedThisTurn = true;      

        if (m_turnEnactedCallback != null)
            m_turnEnactedCallback.Invoke();
    }
}
