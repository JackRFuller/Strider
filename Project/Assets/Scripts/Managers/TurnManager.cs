using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public Action TurnPhaseUpdated;

    private static TurnPhase m_turnPhase = TurnPhase.Movement;
    public static TurnPhase GetTurnPhase { get { return m_turnPhase; } }

    public enum TurnPhase
    {
        Priority = 0,
        Movement,
        Shooting,
        Combat,
    }

    private void Update()
    {
        DebugChangeTurnPhase();
    }

    private void DebugChangeTurnPhase()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.Instance.PhotonView.RPC("UpdateTurnPhase", PhotonTargets.All, 1);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.Instance.PhotonView.RPC("UpdateTurnPhase", PhotonTargets.All, 2);
        }
    }

    [PunRPC]
    private void UpdateTurnPhase(int phase)
    {
        m_turnPhase = (TurnPhase)phase;

        Debug.Log(m_turnPhase);

        if (TurnPhaseUpdated != null)
            TurnPhaseUpdated.Invoke();
    }
}
