using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class UnitMovementGuide : MonoBehaviour
{
    private LineRenderer m_lineRenderer;
    private SpriteRenderer[] m_spriteRenderers;

    [Header("Line Renderer Materials")]
    [SerializeField] private Material m_validPathMaterial;
    [SerializeField] private Material m_invalidPathMaterial;

    [Header("End Points")]
    [SerializeField] private Transform m_startTransform;
    [SerializeField] private Transform m_endTransform;

    private void Start()
    {
        m_lineRenderer = GetComponentInChildren<LineRenderer>();
        m_spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        DisableMovementGuide();
    }

    public void EnableMovementGuide()
    {
        m_lineRenderer.enabled = true;

        for (int spriteRenderer = 0; spriteRenderer < m_spriteRenderers.Length; spriteRenderer++)
        {
            m_spriteRenderers[spriteRenderer].enabled = true;
        }
    }

    public void DisableMovementGuide()
    {
        m_lineRenderer.enabled = false;

        for (int spriteRenderer = 0; spriteRenderer < m_spriteRenderers.Length; spriteRenderer++)
        {
            m_spriteRenderers[spriteRenderer].enabled = false;
        }
    }

    public void SetNavPathVisual(NavMeshPath navMeshPath, bool isValidPath, bool isValidSpot)
    {
        m_lineRenderer.positionCount = navMeshPath.corners.Length;

        for (int corner = 0; corner < navMeshPath.corners.Length; corner++)
        {
            m_lineRenderer.SetPosition(corner, new Vector3(navMeshPath.corners[corner].x,
                                                           navMeshPath.corners[corner].y + 0.05f,
                                                           navMeshPath.corners[corner].z));
        }

        Vector3 startPosition = new Vector3(navMeshPath.corners[0].x,
                                            navMeshPath.corners[0].y + 0.15f,
                                            navMeshPath.corners[0].z);

        Vector3 endPosition = new Vector3(navMeshPath.corners[navMeshPath.corners.Length - 1].x,
                                          navMeshPath.corners[navMeshPath.corners.Length - 1].y + 0.15f,
                                          navMeshPath.corners[navMeshPath.corners.Length - 1].z);


        m_startTransform.position = startPosition;
        m_endTransform.position = endPosition;       


        m_lineRenderer.material = (isValidPath && isValidSpot) ? m_validPathMaterial : m_invalidPathMaterial;

        for (int spriteRenderer = 0; spriteRenderer < m_spriteRenderers.Length; spriteRenderer++)
        {
            m_spriteRenderers[spriteRenderer].color = (isValidPath && isValidSpot) ? Color.blue : Color.red;
        }

        

    }
}
