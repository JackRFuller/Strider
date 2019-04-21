using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitShooting : UnitComponent
{
    public Action UnitShootingStarted;
    public Action UnitFired;
    public Action UnitShootingFinished;

    private Action m_unitShootingComplete;
    private Action m_unitShootingCancelled;

    private RangedWeaponData m_rangedWeaponData;
    private Camera m_playerCamera;
    private UnitShootingGuide m_unitShootingGuide;
    private Transform m_eyeHeight;

    private UnitView m_targetView;
    private Transform m_targetTransform;

    private ShootingPhase m_shootingPhase = ShootingPhase.ShootingToHit;

    private enum ShootingPhase
    {
        ShootingToHit,
        ShootingToWound,
    }

    protected override void Start()
    {
        base.Start();

        m_rangedWeaponData = m_unitView.UnitData.rangedWeapon;
        GetEyeHeight();
        this.enabled = false;
    }

    private void GetEyeHeight()
    {
        foreach(Transform child in transform)
        {
            if(child.CompareTag("EyeHeight"))
            {
                m_eyeHeight = child;
                break;
            }
        }
    }

    private void Update()
    {
        SearchForTarget();

        CancelShooting();
    }

    public void StartUnitShooting(Action unitShootingComplete, Action unitShootingCancelled)
    {
        m_unitShootingComplete = unitShootingComplete;
        m_unitShootingCancelled = unitShootingCancelled;

        if (!m_playerCamera)
            m_playerCamera = m_unitView.PlayerView.PlayerCamera;

        if (m_unitShootingGuide == null)
            m_unitShootingGuide = UnitManager.UnitShootingGuide;

        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        this.enabled = true;

        UnitShootingStarted.Invoke();
    }

    private void SearchForTarget()
    {
        Ray ray = m_playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPosition = Vector3.zero;
        bool hasValidHitPoint = false;
        bool hasValidTarget = false;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.collider.CompareTag("Ground"))
            {
                hasValidHitPoint = true;
                targetPosition = hit.point;
            }

            if (hit.collider.CompareTag("EnemyUnit"))
            {
                if(m_targetTransform == null || m_targetTransform != hit.transform)
                {
                    m_targetTransform = hit.transform;
                    m_targetView = m_targetTransform.GetComponent<UnitView>();
                }

                hasValidTarget = IsValidTarget(m_targetTransform);
                hasValidHitPoint = true;
                targetPosition = m_targetTransform.position;
                if (hasValidTarget)
                {
                    if(Input.GetMouseButton(0))
                    {
                        DiceRollEvent diceRollEvent = new DiceRollEvent(m_playerCamera, m_rangedWeaponData.numberOfShots, RecieveDiceResults);
                        GameManager.Instance.DiceManager.InitiateDiceEvent(diceRollEvent);

                        this.enabled = false;
                    }
                }
            }

            TurnUnitToFaceTarget(targetPosition);
        }

        m_unitShootingGuide.SetShootingGuide(transform.position,targetPosition, hasValidTarget, hasValidHitPoint);
    }

    private void TurnUnitToFaceTarget(Vector3 targetPosition)
    {
        if(targetPosition != Vector3.zero)
        {
            Vector3 lookAt = new Vector3(targetPosition.x, 0, targetPosition.z);
            transform.LookAt(lookAt);
        }
    }

    private void RecieveDiceResults(List<int> diceResults)
    {       
        if(m_shootingPhase == ShootingPhase.ShootingToHit)
        {
            int numberOfHits = 0;

            for(int i =0; i < diceResults.Count; i++)
            {
                if (diceResults[i] >= 6 - m_unitView.UnitData.shootingSkill)
                {
                    numberOfHits++;
                }
            }

            if(numberOfHits > 0)
            {
                DiceRollEvent diceRollEvent = new DiceRollEvent(m_playerCamera, numberOfHits, RecieveDiceResults);
                GameManager.Instance.DiceManager.InitiateDiceEvent(diceRollEvent);
                m_shootingPhase = ShootingPhase.ShootingToWound;
            }
            else if(numberOfHits == 0)
            {
                UnitShootingFinished.Invoke();

                m_unitShootingComplete.Invoke();
                m_unitShootingGuide.DisableShootingGuide();
            }
        }
        else if(m_shootingPhase == ShootingPhase.ShootingToWound)
        {
            int diceRollNeeded = WoundChart.ValueNeededToCauseAWound(m_targetView.UnitData.defense, m_unitView.UnitData.strength);

            int numberOfWoundsCaused = 0;

            for (int j = 0; j < diceResults.Count; j++)
            {
                if (diceResults[j] >= 0)
                {
                    numberOfWoundsCaused++;
                }
            }

            if (numberOfWoundsCaused > 0)            
                m_targetView.UnitHealth.RemoveHealthPoints(numberOfWoundsCaused);
            

            if(UnitFired != null)
                UnitFired.Invoke();           

            m_unitShootingComplete.Invoke();
            m_unitShootingGuide.DisableShootingGuide();
        }
    }

    private bool IsValidTarget(Transform target)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if(distanceToTarget <= m_rangedWeaponData.weaponRange)
        {
            //Check if we have line of sight
            Vector3 targetPosition = target.position;
            targetPosition.y += 1;
            Vector3 direction = targetPosition - m_eyeHeight.position;

            Ray ray = new Ray(m_eyeHeight.position, direction);
            Debug.DrawRay(m_eyeHeight.position, direction, Color.red, 10);

            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.transform == target)
                {                  
                    return true;
                }
                else
                {                   
                    return false;
                }
            }
             
        }

        return false;
    }

    private void CancelShooting()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if (m_unitShootingCancelled != null)
                m_unitShootingCancelled.Invoke();

            this.gameObject.layer = LayerMask.NameToLayer("Unit");
            this.enabled = false;

            UnitShootingFinished.Invoke();
            m_unitShootingGuide.DisableShootingGuide();
        }
    }
}
