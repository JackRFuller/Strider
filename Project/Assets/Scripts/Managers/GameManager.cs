﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private PhotonView m_photonView;
    private TurnManager m_turnManager;
    private UnitManager m_unitManager;
    private CombatManager m_combatManager;
    private DiceManager m_diceManager;

    public TurnManager TurnManager { get { return m_turnManager; } }
    public PhotonView PhotonView { get { return m_photonView; } }
    public UnitManager UnitManager { get { return m_unitManager; } }
    public CombatManager CombatManager { get { return m_combatManager; } }
    public DiceManager DiceManager { get { return m_diceManager; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_photonView = GetComponent<PhotonView>();
        m_turnManager = GetComponent<TurnManager>();
        m_unitManager = GetComponent<UnitManager>();
        m_combatManager = GetComponent<CombatManager>();
        m_diceManager = GetComponent<DiceManager>();
    }
}
