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

    public UnitData UnitData { get { return m_unitData; } }
    public PhotonView PhotonView { get { return m_photonView; } }
    public UnitBehaviour UnitBehaviour { get { return m_unitBehaviour; } }
    public UnitMovement UnitMovement { get { return m_unitMovement; } }
    public UnitShooting UnitShooting { get { return m_unitShooting; } }
    public UnitFieldOfView UnitFieldOfView { get { return m_unitFieldOfView; } }

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

        if (!m_photonView.isMine)
            this.gameObject.tag = "EnemyUnit";
    }

    public void SetPlayerView(PlayerView playerView)
    {
        if (m_playerView == null)
            m_playerView = playerView; 
    }
}
