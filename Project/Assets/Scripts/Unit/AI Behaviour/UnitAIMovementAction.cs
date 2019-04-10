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

        if(IsThereAValidPathToTarget())
        {
            Debug.Log("Found Valid Path");

            if (IsTargetInRange())
            {
                Vector3 targetPosition = GetValidMovementPosition(m_targetUnit.transform.position);

                m_navAgent.destination = targetPosition;
                this.enabled = true;
            }
            else
            {
                //Get as Close As Possible
            }
        }
        else
        {

        }
    }

    private bool IsTargetInRange()
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

    private bool IsThereAValidPathToTarget()
    {
        NavMesh.CalculatePath(transform.position, m_unitView.transform.position, NavMesh.AllAreas, m_navPath);

        if(m_navPath.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private Vector3 GetValidMovementPosition(Vector3 targetPosition)
    {
        Vector3 movementPosition = targetPosition;

        for (int x = -1; x < 1; x++)
        {
            for(int y = -1; y < 1; y++)
            {
                movementPosition = new Vector3(targetPosition.x + x, 0, targetPosition.z + y);

                if(movementPosition.x != targetPosition.x && movementPosition.z != targetPosition.z)
                {
                    Collider[] hitColliders = Physics.OverlapSphere(movementPosition, 0.5f);

                    bool objectInTheWay = false;

                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        if (hitColliders[i].gameObject.name != "Ground")
                        {
                            objectInTheWay = true;
                            Debug.Log("Hit " + hitColliders[i].gameObject.name);
                            break;
                        }
                    }

                    if (!objectInTheWay)
                    {
                        return movementPosition;
                    }
                }
            }
        }

        return movementPosition;
    }

    private void CheckUnitHasReachedTheirDestination()
    {
        if(!m_navAgent.pathPending)
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
}
