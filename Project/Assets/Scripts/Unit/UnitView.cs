using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField]
    private UnitData m_unitData;

    private PlayerView m_playerView;

    private PhotonView m_photonView;
    private UnitBehaviour m_unitBehaviour;
    private UnitMovement m_unitMovement;
    private UnitShooting m_unitShooting;
    private UnitFieldOfView m_unitFieldOfView;

    //AI Behaviours
    private UnitAIBehaviour m_unitAIBehaviour;
    private UnitAIMovementAction m_unitAIMoveAction;
    private UnitAIShootingAction m_unitAIShootAction;

    public UnitData UnitData { get { return m_unitData; } }
    public PhotonView PhotonView { get { return m_photonView; } }
    public UnitBehaviour UnitBehaviour { get { return m_unitBehaviour; } }
    public UnitMovement UnitMovement { get { return m_unitMovement; } }
    public UnitShooting UnitShooting { get { return m_unitShooting; } }
    public UnitFieldOfView UnitFieldOfView { get { return m_unitFieldOfView; } }

    //Public AI Behaviours
    public UnitAIBehaviour UnitAIBehaviour { get { return m_unitAIBehaviour; } }
    public UnitAIMovementAction UnitAIMoveAction { get { return m_unitAIMoveAction; } }
    public UnitAIShootingAction UnitAIShootingAction { get { return m_unitAIShootAction; } }

    public PlayerView PlayerView { get { return m_playerView; } }
   

    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
        m_unitBehaviour = GetComponent<UnitBehaviour>();
        m_unitMovement = GetComponent<UnitMovement>();
        m_unitShooting = GetComponent<UnitShooting>();
        m_unitFieldOfView = GetComponent<UnitFieldOfView>();
    }

    private void Start()
    {
        UnitManager.AddUnit(this);

        //if (!m_photonView.isMine)
        //    this.gameObject.tag = "EnemyUnit";
    }

    public void SetPlayerView(PlayerView playerView)
    {
        if (m_playerView == null)
            m_playerView = playerView; 
    }

    public void SetAsAIUnit()
    {
        this.gameObject.tag = "EnemyUnit";

        GameManager.Instance.UnitAIManager.AddUnit(this);
        GetComponent<UnitBase>().SetBaseColourRed();

        //Add AI Behaviours
        m_unitAIBehaviour = this.gameObject.AddComponent<UnitAIBehaviour>();
        m_unitAIMoveAction = this.gameObject.AddComponent<UnitAIMovementAction>();
        m_unitAIShootAction = this.gameObject.AddComponent<UnitAIShootingAction>();

        Destroy(m_unitBehaviour);
        Destroy(m_unitMovement);
        Destroy(m_unitShooting);
    }
}
