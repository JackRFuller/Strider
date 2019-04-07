using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private GameObject m_unitMovementGuidePrefab;
    [SerializeField] private GameObject m_unitShootingGuidePrefab;

    private static List<UnitView> m_units;

    private static UnitMovementGuide m_unitMovementGuide;
    private static UnitShootingGuide m_unitShootingGuide;

   
    public static List<UnitView> Units { get { return m_units; } }
    public static UnitMovementGuide UnitMovementGuide { get { return m_unitMovementGuide; } }
    public static UnitShootingGuide UnitShootingGuide { get { return m_unitShootingGuide; } }

    private void Awake()
    {
        m_units = new List<UnitView>();
    }

    private void Start()
    {
        GameObject movementGuide = Instantiate(m_unitMovementGuidePrefab);
        m_unitMovementGuide = movementGuide.GetComponent<UnitMovementGuide>();

        GameObject shootingGuide = Instantiate(m_unitShootingGuidePrefab);
        m_unitShootingGuide = shootingGuide.GetComponent<UnitShootingGuide>();
    }

    public static void AddUnit(UnitView unit)
    {
        m_units.Add(unit);
    }

    //Called from Unit
    public void UnitStartedMoving()
    {
        for (int unit = 0; unit < m_units.Count; unit++)
        {
            if(!m_units[unit].PhotonView.isMine)
            {
                m_units[unit].UnitFieldOfView.EnableFieldOfView();
            }
        }       
    }

    //Called from Unit
    public void UnitStoppedMoving()
    {
        for (int unit = 0; unit < m_units.Count; unit++)
        {
            if (!m_units[unit].PhotonView.isMine)
            {
                m_units[unit].UnitFieldOfView.DisableFieldOfView();
            }
        }
    }

}
