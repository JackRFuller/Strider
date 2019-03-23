using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_unitMovementGuidePrefab;

    private static List<UnitView> units;
    private static UnitMovementGuide m_unitMovementGuide;

   
    public static List<UnitView> Units { get { return units; } }
    public static UnitMovementGuide UnitMovementGuide { get { return m_unitMovementGuide; } }

    private void Awake()
    {
        units = new List<UnitView>();
    }

    private void Start()
    {
        GameObject movementGuide = Instantiate(m_unitMovementGuidePrefab);
        m_unitMovementGuide = movementGuide.GetComponent<UnitMovementGuide>();
    }

    public static void AddUnit(UnitView unit)
    {
        units.Add(unit);
    }
}
