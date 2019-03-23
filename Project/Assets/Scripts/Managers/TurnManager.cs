using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private static TurnPhase m_turnPhase = TurnPhase.Movement;
    public static TurnPhase GetTurnPhase { get { return m_turnPhase; } }

    public enum TurnPhase
    {
        Priority,
        Movement,
        Shooting,
        Combat,
    }

}
