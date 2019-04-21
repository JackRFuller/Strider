using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShootingGuide : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private LineRenderer m_lineRenderer;

    [Header("Line Renderer Materials")]
    [SerializeField] private Material m_validPathMaterial;
    [SerializeField] private Material m_invalidPathMaterial;


    // Start is called before the first frame update
    void Start()
    {
        m_lineRenderer = GetComponentInChildren<LineRenderer>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        DisableShootingGuide();
    }

    public void EnableShootingGuide()
    {
        m_spriteRenderer.enabled = true;
        m_lineRenderer.enabled = true;
    }

    public void DisableShootingGuide()
    {
        m_spriteRenderer.enabled = false;
        m_lineRenderer.enabled = false;
    }

    public void SetShootingGuide(Vector3 shooterPosition, Vector3 shotPosition, bool hasValidTarget, bool validHitPoint)
    {
        if(validHitPoint)
        {
            transform.position = shotPosition;   
            
            m_spriteRenderer.color = hasValidTarget ? Color.blue : Color.red;
            m_lineRenderer.material = hasValidTarget ? m_validPathMaterial : m_invalidPathMaterial;

            if (!m_spriteRenderer.enabled)
                m_spriteRenderer.enabled = true;

            if (!m_lineRenderer.enabled)
                m_lineRenderer.enabled = true;

            Vector3 positionOne = new Vector3(shooterPosition.x,
                                              shooterPosition.y + 0.05f,
                                              shooterPosition.z);

            m_lineRenderer.SetPosition(0, positionOne);

            Vector3 distanceVector = shotPosition - shooterPosition;          
            Vector3 distanceVectorNormalized = distanceVector.normalized;         

            float distance = Vector3.Distance(positionOne, shotPosition);
            distance -= 1f;

            Vector3 targetPosition = positionOne + (distanceVectorNormalized * distance);

            Vector3 positionTwo = new Vector3(targetPosition.x,
                                              targetPosition.y + 0.05f,
                                              targetPosition.z);

            m_lineRenderer.SetPosition(1, positionTwo);

        }
        else
        {
            if(m_spriteRenderer.enabled)
                m_spriteRenderer.enabled = false;

            if (m_lineRenderer.enabled)
                m_lineRenderer.enabled = false;
        }
    }

    
}
