using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField]
    private UnitData m_unitData;

    private int m_teamID;

    private PlayerView m_playerView;

    private UnitBehaviour m_unitBehaviour;
    private UnitMovement m_unitMovement;
    private UnitShooting m_unitShooting;
    private UnitFieldOfView m_unitFieldOfView;
    private UnitCombat m_unitCombat;
    private UnitHealth m_unitHealth;

    //AI Behaviours
    private UnitAIBehaviour m_unitAIBehaviour;
    private UnitAIMovementAction m_unitAIMoveAction;
    private UnitAIShootingAction m_unitAIShootAction;

    public UnitData UnitData { get { return m_unitData; } }
    public UnitBehaviour UnitBehaviour { get { return m_unitBehaviour; } }
    public UnitMovement UnitMovement { get { return m_unitMovement; } }
    public UnitShooting UnitShooting { get { return m_unitShooting; } }
    public UnitFieldOfView UnitFieldOfView { get { return m_unitFieldOfView; } }
    public UnitCombat UnitCombat { get { return m_unitCombat; } }
    public UnitHealth UnitHealth { get { return m_unitHealth; } }


    //Public AI Behaviours
    public UnitAIBehaviour UnitAIBehaviour { get { return m_unitAIBehaviour; } }
    public UnitAIMovementAction UnitAIMoveAction { get { return m_unitAIMoveAction; } }
    public UnitAIShootingAction UnitAIShootingAction { get { return m_unitAIShootAction; } }

    public int TeamID { get { return m_teamID; } }

    public PlayerView PlayerView { get { return m_playerView; } }
   

    private void Awake()
    {
        m_unitBehaviour = GetComponent<UnitBehaviour>();
        m_unitMovement = GetComponent<UnitMovement>();
        m_unitShooting = GetComponent<UnitShooting>();
        m_unitFieldOfView = GetComponent<UnitFieldOfView>();
        m_unitCombat = this.gameObject.AddComponent<UnitCombat>();
        m_unitHealth = this.gameObject.AddComponent<UnitHealth>();

        gameObject.AddComponent<UnitAnimation>();

        //Spawn in Model
        GameObject unitModel = Instantiate(UnitData.unitModel);
        unitModel.transform.parent = this.transform;
        unitModel.transform.localPosition = new Vector3(0, 0.1f, 0);
        unitModel.transform.rotation = transform.rotation;
    }

    private void Start()
    {
        UnitManager.AddUnit(this);
    }

    public void SetTeam(int teamID)
    {
        m_teamID = teamID;

        if(m_teamID != 1)
        {
            //SetAsAIUnit
            SetAsAIUnit();
        }
    }

    public void SetPlayerView(PlayerView playerView)
    {
        if (m_playerView == null)
            m_playerView = playerView; 
    }

    public void SetAsAIUnit()
    {
        gameObject.tag = "EnemyUnit";

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
