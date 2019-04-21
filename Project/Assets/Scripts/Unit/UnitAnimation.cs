using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimation : UnitComponent
{
    private Animator m_animator;
   
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        m_animator = GetComponentInChildren<Animator>();

        m_unitView.UnitShooting.UnitShootingStarted += SetShooting;
        m_unitView.UnitShooting.UnitFired += FireBow;
        m_unitView.UnitShooting.UnitShootingFinished += FinishShooting;

        m_unitView.UnitHealth.UnitDied += UnitDied;
    }

    #region Shooting

    private void SetShooting()
    {
        m_animator.SetBool("isShooting", true);
    }

    private void FireBow()
    {
        m_animator.SetTrigger("FireBow");
        Action cooldownFunction = FinishShooting;
        StartCoroutine(AnimationCooldown(cooldownFunction, 0.5f));        
    }   

    private void FinishShooting()
    {
        m_animator.SetBool("isShooting", false);
    }

    #endregion

    private void UnitDied()
    {
        m_animator.SetTrigger("Died");
    }

    IEnumerator AnimationCooldown(Action functionAfterCooldown, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        functionAfterCooldown.Invoke();
    }
}
