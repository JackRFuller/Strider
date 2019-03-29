using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    public Action UnitStartedMovement;
    public Action UnitEndedMovement;

    [SerializeField]
    private GameObject m_unitMovementGuidePrefab;

    private static List<UnitView> m_units;
    private static UnitMovementGuide m_unitMovementGuide;

   
    public static List<UnitView> Units { get { return m_units; } }
    public static UnitMovementGuide UnitMovementGuide { get { return m_unitMovementGuide; } }

    private void Awake()
    {
        m_units = new List<UnitView>();
    }

    private void Start()
    {
        GameObject movementGuide = Instantiate(m_unitMovementGuidePrefab);
        m_unitMovementGuide = movementGuide.GetComponent<UnitMovementGuide>();
    }

    public static void AddUnit(UnitView unit)
    {
        m_units.Add(unit);
    }

    public void UnitStartedMoving()
    {
        if (UnitStartedMovement != null)
            UnitStartedMovement.Invoke();
    }

    public void UnitStoppedMoving()
    {
        if (UnitEndedMovement != null)
            UnitEndedMovement.Invoke();
    }

}
