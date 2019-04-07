using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShootingGuide : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        DisableShootingGuide();
    }

    public void EnableShootingGuide()
    {
        m_spriteRenderer.enabled = true;
    }

    public void DisableShootingGuide()
    {
        m_spriteRenderer.enabled = false;
    }

    public void SetShootingGuide(Vector3 shotPosition, bool hasValidTarget, bool validHitPoint)
    {
        if(validHitPoint)
        {
            transform.position = shotPosition;            
            m_spriteRenderer.color = hasValidTarget ? Color.blue : Color.red;

            if (!m_spriteRenderer.enabled)
                m_spriteRenderer.enabled = true;
        }
        else
        {
            if(m_spriteRenderer.enabled)
                m_spriteRenderer.enabled = false;
        }
    }

    
}
