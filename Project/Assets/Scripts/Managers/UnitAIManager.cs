using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAIManager : MonoBehaviour
{
    private Action m_unitTurnFinished;

    private List<UnitView> m_aiUnits = new List<UnitView>();

    private int unitIndex = -1;

    private void Start()
    {
        m_unitTurnFinished = InitiateTurn;

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(3.0f);
        InitiateTurn();
    }

    public void AddUnit(UnitView aiUnit)
    {
        m_aiUnits.Add(aiUnit);       
    }

    void InitiateTurn()
    {
        if(unitIndex < m_aiUnits.Count - 1)
        {
            unitIndex++;
            m_aiUnits[unitIndex].UnitAIBehaviour.StartTurnAction(m_unitTurnFinished);
        }
    }
       

    private int GetUnitToMove()
    {
        return unitIndex++;
    }

    public UnitView GetTarget()
    {
        int randomTargetIndex = UnityEngine.Random.Range(0, UnitManager.Units.Count);

        while(UnitManager.Units[randomTargetIndex].CompareTag("EnemyUnit"))
        {
            randomTargetIndex = UnityEngine.Random.Range(0, UnitManager.Units.Count);
        }

        return UnitManager.Units[randomTargetIndex];
    }
}
