using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    //Temp - Refactor
    public UIMatchMessage uiMatchMessage;

    public Action TurnPhaseUpdated;

    private MatchMessage m_matchMessage;
    private int m_turnCount = 0;

    private static TurnPhase m_turnPhase = TurnPhase.Movement;
    public static TurnPhase GetTurnPhase { get { return m_turnPhase; } }

    public enum TurnPhase
    {
        Priority = 0,
        Movement,
        Shooting,
        Combat,
    }

    private void Start()
    {
        m_turnPhase = TurnPhase.Movement;
        m_matchMessage = new MatchMessage();
    }

    private void Update()
    {
        DebugChangeTurnPhase();
    }

    public void EndTurn()
    {
        if (m_turnPhase == TurnPhase.Priority)
        {
            m_turnPhase = TurnPhase.Movement;
            m_turnCount = 0;

            m_matchMessage.matchMessageHeader = "- Your Turn -";
            m_matchMessage.matchMessageSubHeader = "Movement Phase";
        }
        else if (m_turnPhase == TurnPhase.Movement || m_turnPhase == TurnPhase.Shooting)
        {
            if (m_turnCount == 0)
            {
                m_turnCount++;
                m_matchMessage.matchMessageHeader = "- Enemy's Turn -";
                m_matchMessage.matchMessageSubHeader = m_turnPhase == TurnPhase.Movement ? "Movement Phase" : "Shooting Phase";                
            }                
            else
            {
                m_turnPhase = m_turnPhase == TurnPhase.Movement ? TurnPhase.Shooting : TurnPhase.Combat;

                Debug.Log(m_turnPhase);

                if(m_turnPhase == TurnPhase.Shooting)
                {
                    m_matchMessage.matchMessageHeader = "- Your Turn -";
                    m_matchMessage.matchMessageSubHeader = "Shooting Phase";
                    m_turnCount = 0;
                }
                else if(m_turnPhase == TurnPhase.Combat)
                {
                    m_matchMessage.matchMessageHeader = "Combat Phase";
                    m_matchMessage.matchMessageSubHeader = null;
                }
            }
        }
        else if (m_turnPhase == TurnPhase.Combat)
        {
            m_turnPhase = TurnPhase.Priority;

            m_matchMessage.matchMessageHeader = "Priority Phase";
            m_matchMessage.matchMessageSubHeader = null;
        }

        uiMatchMessage.ShowMatchMessage(m_matchMessage);

        if (TurnPhaseUpdated != null)
            TurnPhaseUpdated.Invoke();
    }

    public bool IsPlayersTurn()
    {
        if (m_turnPhase == TurnPhase.Movement || m_turnPhase == TurnPhase.Shooting)
        {
            if (m_turnCount == 0)
                return true;
            else
                return false;
        }

        return false;
    }

    #region Debug

    private void DebugChangeTurnPhase()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateTurnPhase(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpdateTurnPhase(2);
        }
    }
   
    private void UpdateTurnPhase(int phase)
    {
        m_turnPhase = (TurnPhase)phase;

        if (TurnPhaseUpdated != null)
            TurnPhaseUpdated.Invoke();
    }

    #endregion

}
