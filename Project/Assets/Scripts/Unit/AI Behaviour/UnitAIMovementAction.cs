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

    private const int m_searchRadius = 1;
    private const int m_maxNumberOfChecks = 8;

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
        Vector3 targetPosition = GetClosestPositionToTargetUnit(m_targetUnit.transform.position); 

        m_navAgent.destination = targetPosition;
        this.enabled = true;
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

    private Vector3 GetClosestPositionToTargetUnit(Vector3 targetUnitPosition)
    {
        bool hasFoundValidPosition = false;

        Vector3 newMovePosition = Vector3.zero;
        Vector3 closestMovePosition = Vector3.zero;        

        for (int radius = 1; radius < m_unitView.UnitData.maxMovementDistancePerTurn; radius++)
        {
            Vector3 searchStartPoint = GetSearchStartPosition(targetUnitPosition, radius);                     
            
            for (int i = 0; i < m_maxNumberOfChecks; i++)
            {
                float angle = i * Mathf.PI * 2f / m_maxNumberOfChecks;
                Vector3 circlePosition = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

                newMovePosition = searchStartPoint + circlePosition;
               
                if (IsValidMovementPosition(newMovePosition))
                {
                    if (closestMovePosition == Vector3.zero)
                    {
                        closestMovePosition = newMovePosition;
                    }
                    else
                    {
                       if(IsPositionCloser(newMovePosition, closestMovePosition))
                            closestMovePosition = newMovePosition;
                    }

                    hasFoundValidPosition = true;
                }
            }

            if (hasFoundValidPosition)
                break;
        }

        return closestMovePosition;
    }

    private bool IsPositionCloser(Vector3 newPositon, Vector3 oldPosition)
    {
        float distanceToNewPosition = Vector3.Distance(transform.position, newPositon);
        float distanceToOldPosition = Vector3.Distance(transform.position, oldPosition);

        if (distanceToNewPosition < distanceToOldPosition)
            return true;

        return false;
    }

    private Vector3 GetSearchStartPosition(Vector3 targetUnitPosition, int radius)
    {
        Vector3 searchStartPosition = targetUnitPosition;

        //Check if Target is Within Range
        if (!IsTargetWithinMovementRange(targetUnitPosition))
        {
            Vector3 targetVector = targetUnitPosition - transform.position;
            targetVector.Normalize();
            searchStartPosition = transform.position + (targetVector * (m_unitView.UnitData.maxMovementDistancePerTurn - radius));           
        }        

        return searchStartPosition;
    }

    private bool IsValidMovementPosition(Vector3 targetPosition)
    {
        //Check We're On The Board
        if (IsPositionWithinConfinesOfTheBoard(targetPosition))
        {
            //Check There's Nothing Already There
            if (IsPositionClear(targetPosition))
            {
                //Check There is A Valid NavMesh Path
                if (IsThereAValidPathBetweenUnitAndTarget(targetPosition))
                {
                    //Check it's within range
                    if (IsTargetWithinMovementRange(targetPosition))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool IsNewPositionCloser(Vector3 oldmovePosition, Vector3 newMovePosition)
    {
        float distance = Vector3.Distance(transform.position, oldmovePosition);
        float distance2 = Vector3.Distance(transform.position, newMovePosition);

        if (distance2 < distance)
        {
            return true;
        }

        return false;
          
    }

    private bool IsThereAValidPathBetweenUnitAndTarget(Vector3 targetPosition)
    {
        NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, m_navPath);

        bool isValidPath = m_navPath.status == NavMeshPathStatus.PathInvalid ? false : true;

        return isValidPath;
    }

    private bool IsTargetWithinMovementRange(Vector3 targetPosition)
    {
        float pathLength = 0;

        NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, m_navPath);

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
        NavMesh.CalculatePath(transform.position, m_navAgent.destination, NavMesh.AllAreas, m_navPath);

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
