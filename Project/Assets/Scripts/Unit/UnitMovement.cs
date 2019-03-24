using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class UnitMovement : UnitBehaviour
{
    private Action m_unitMovementComplete;
    private Action m_unitMovementCancelled;

    private UnitMovementGuide m_unitMovementGuide;
    private NavMeshPath m_navMeshPath;
    private Collider m_unitCollider;
    
    private Camera m_playerCamera;
    private Vector3 m_originalPosition;

    protected override void Start()
    {
        base.Start();

        m_navMeshPath = new NavMeshPath();
        m_unitCollider = GetComponent<Collider>();
        this.enabled = false;
    }

    private void Update()
    {
        MoveUnit();

        CancelMove();
    }

    public void StartUnitMovement(Action unitMovementComplete, Action unitMovementCancelled)
    {
        if (m_unitMovementGuide == null)
            m_unitMovementGuide = UnitManager.UnitMovementGuide;

        m_unitMovementComplete = unitMovementComplete;
        m_unitMovementCancelled = unitMovementCancelled;

        if(!m_playerCamera)
            m_playerCamera = m_unitView.PlayerView.PlayerCamera;

        m_originalPosition = transform.position;
        
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        m_unitMovementGuide.EnableMovementGuide();
        m_unitCollider.enabled = false;
        Cursor.visible = false;
        this.enabled = true;
    }

   
    private void MoveUnit()
    {
        Ray ray = m_playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool isValidPath = false;
        bool isValidPoint = false;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.collider.CompareTag("Ground"))
            {
                Vector3 targetPosition = hit.point;
                transform.position = GetHoverPosition(targetPosition);

                NavMesh.CalculatePath(m_originalPosition, targetPosition, NavMesh.AllAreas, m_navMeshPath);

                isValidPoint = IsValidSpot(targetPosition);
                isValidPath = IsValidPath(targetPosition);

                //Determine if valid path
                if(isValidPoint && isValidPath)
                {   
                    if (Input.GetMouseButtonDown(0))
                    {
                        MoveToPosition(hit.point);
                    }
                }                
            }
        }

        m_unitMovementGuide.SetNavPathVisual(m_navMeshPath, isValidPath, isValidPoint);
    }

    private Vector3 GetHoverPosition(Vector3 hitPoint)
    {
        Vector3 hoverPosition = hitPoint;
        hoverPosition.y += 1;
        return hoverPosition;
    }

    private bool IsValidPath(Vector3 targetPosition)
    {
        if(m_navMeshPath.status != NavMeshPathStatus.PathInvalid)
        {
            float pathLength = 0;

            for (int cornerIndex = 1; cornerIndex < m_navMeshPath.corners.Length; cornerIndex++)
            {
                pathLength += Vector3.Distance(m_navMeshPath.corners[cornerIndex - 1], m_navMeshPath.corners[cornerIndex]);
            }

            if (pathLength <= m_unitView.UnitData.maxMovementDistancePerTurn)
                return true;
        }

        return false;
    }

    //Check there is no collision
    private bool IsValidSpot(Vector3 targetPosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, 0.5f);

        bool isObjectInTheWay = true;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.name != "Ground")
            {
                isObjectInTheWay = false;
                Debug.Log("Hit " + hitColliders[i].gameObject.name);
                break;
            }            
        }

        return isObjectInTheWay;
    }

    private void MoveToPosition(Vector3 hitPoint)
    {
        transform.position = hitPoint;
        m_unitCollider.enabled = true;

        ResetUnitAfterCancelledOrCompletedMove();

        if (m_unitMovementComplete != null)
            m_unitMovementComplete.Invoke();
    }

    private void CancelMove()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if (m_unitMovementCancelled != null)
                m_unitMovementCancelled();            

            transform.position = m_originalPosition;

            ResetUnitAfterCancelledOrCompletedMove();
        }
    }

    private void ResetUnitAfterCancelledOrCompletedMove()
    {
        m_unitMovementGuide.DisableMovementGuide();
        gameObject.layer = LayerMask.NameToLayer("Unit");
        Cursor.visible = true;
        this.enabled = false;
    }
}
