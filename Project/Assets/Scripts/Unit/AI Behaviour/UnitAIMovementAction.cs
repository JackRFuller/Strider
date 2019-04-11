using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class UnitAIMovementAction : UnitAIAction
{
    private UnitView m_targetUnit;
    

    private NavMeshAgent m_navAgent;
    private NavMeshPath m_navPath;

    protected override void Start()
    {
        base.Start();

        m_navAgent = GetComponent<NavMeshAgent>();
        m_navPath = new NavMeshPath();

        this.enabled = false;
    }

    private void Update()
    {
        CheckUnitHasReachedTheirDestination();
    }


    public override void DoAction(Action turnFinished)
    {
        base.DoAction(turnFinished);

        m_targetUnit = m_unitView.UnitAIBehaviour.PrimaryTargetUnit;

        switch (m_unitView.UnitAIBehaviour.AIType)
        {
            case UnitAIBehaviour.AIBehaviourType.Aggressive:
                AggressiveMove();
                break;
            case UnitAIBehaviour.AIBehaviourType.Defensive:
                DefensiveMove();
                break;
        }
    }

    private void AggressiveMove()
    {
        Vector3 targetPosition = Vector3.zero;

        targetPosition = GetClosestPositionToTargetUnit(m_targetUnit.transform.position);        

        Debug.Log("Target unit Pos:" + m_targetUnit.transform.position + "Target Pos" + targetPosition,m_targetUnit.gameObject);

        if(targetPosition != Vector3.zero)
        {
            m_navAgent.destination = targetPosition;
            this.enabled = true;
        }
        else
        {
            if (m_actionFinished != null)
                m_actionFinished.Invoke();
        }
    }

    private void DefensiveMove()
    {
        //Can Target Reach me            

        //If So Try and Move Out of Way

        //Can we move to somewhere in range

        //If not calculate area 

        if (m_actionFinished != null)
            m_actionFinished.Invoke();
    } 

    private void GetFurthestDistanceAwayFromTargetUnit()
    {
        
    }

    private Vector3 GetClosestPositionToTargetUnit(Vector3 targetPosition)
    {
        int xBounds = 0;
        int zBounds = 0;

        for (int i = 0; i < BoardManager.Rows * BoardManager.Columns;i++)
        {           
            xBounds++;
            zBounds++;
           
            for (int z = zBounds; z > -zBounds; z--)
            {
                for (int x = xBounds; x > -xBounds; x--)
                {
                    Vector3 movePosition = new Vector3(targetPosition.x + x, 0, targetPosition.z + z);                                      

                    //Check We're On The Board
                    if (IsPositionWithinConfinesOfTheBoard(movePosition))
                    {                        
                        //Check There is A Valid Path
                        if (IsThereAValidPathBetweenUnitAndTarget(movePosition))
                        {
                            //Check it's withing range
                            if (IsTargetWithinMovementRange())
                            {
                                //Check There's Nothing Already There
                                if (IsPositionClear(movePosition))
                                {
                                    //Check There
                                    return movePosition;
                                }
                            }
                        }                        
                    }
                }
            }
        }       

        return Vector3.zero;
    }

    private bool IsThereAValidPathBetweenUnitAndTarget(Vector3 targetPosition)
    {
        NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, m_navPath);

        if (m_navPath.status != NavMeshPathStatus.PathInvalid)
        {
            return true;
        }

        return false;
    }

    private bool IsTargetWithinMovementRange()
    {
        float pathLength = 0;

        for (int cornerIndex = 1; cornerIndex < m_navPath.corners.Length; cornerIndex++)
        {
            pathLength += Vector3.Distance(m_navPath.corners[cornerIndex - 1], m_navPath.corners[cornerIndex]);
        }

        if (pathLength <= m_unitView.UnitData.maxMovementDistancePerTurn)
            return true;
        else
            return false;
    }

    private bool IsPositionClear(Vector3 targetPosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, 0.5f);

        bool spotIsClear = true;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.name != "Ground")
            {
                spotIsClear = false;
            }
        }

        return spotIsClear;
    }

    private void CheckUnitHasReachedTheirDestination()
    {
        for (int i = 0; i < m_navPath.corners.Length - 1; i++)
        {
            Debug.DrawLine(m_navPath.corners[i], m_navPath.corners[i + 1], Color.red);
        }

        if (!m_navAgent.pathPending)
        {
            if(m_navAgent.remainingDistance <= m_navAgent.stoppingDistance)
            {
                if(!m_navAgent.hasPath || m_navAgent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Reached Destination");  

                    if (m_actionFinished != null)
                        m_actionFinished.Invoke();

                    this.enabled = false;
                }
            }
        }
    }

    private bool IsPositionWithinConfinesOfTheBoard(Vector3 position)
    {
        if(position.x >= 0 && position.x <= BoardManager.Columns)
        {
            if(position.z >= 0 && position.z <= BoardManager.Rows)
            {
                return true;
            }
        }

        return false;
    }
}
