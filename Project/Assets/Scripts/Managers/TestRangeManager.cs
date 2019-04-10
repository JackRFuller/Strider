using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangeManager : MonoBehaviour
{
    [SerializeField] private GameObject m_player;
    
    [Header("Units")]
    [SerializeField] private GameObject m_playerUnit;
    [SerializeField] private GameObject m_AIUnit;

    [Header("Level Spawn Points")]
    [SerializeField] private Transform[] m_playerSpawnPoints;
    [SerializeField] private Transform[] m_AIUnitSpawnPoints;

    private void Start()
    {
        SpawnInPlayer();

        SpawnInPlayerUnits();

        SpawnInAIUnits();
    }

    private void SpawnInPlayer()
    {
        GameObject player = Instantiate(m_player,new Vector3(15,10,0),Quaternion.Euler(new Vector3(45,0,0)));
    }

    private void SpawnInPlayerUnits()
    {
        for (int spawnIndex = 0; spawnIndex < m_playerSpawnPoints.Length; spawnIndex++)
        {
            GameObject unit = Instantiate(m_playerUnit, m_playerSpawnPoints[spawnIndex].position,m_playerSpawnPoints[spawnIndex].rotation);
        }
    }

    private void SpawnInAIUnits()
    {
        for (int spawnIndex = 0; spawnIndex < m_AIUnitSpawnPoints.Length; spawnIndex++)
        {
            GameObject unit = Instantiate(m_AIUnit, m_AIUnitSpawnPoints[spawnIndex].position, m_AIUnitSpawnPoints[spawnIndex].rotation);
            UnitView unitView = unit.GetComponent<UnitView>();
            unitView.SetAsAIUnit();
        }
    }
}
