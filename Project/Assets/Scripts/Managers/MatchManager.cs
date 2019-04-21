using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [SerializeField] private GameObject m_playerPrefab;

    [SerializeField] private GameObject m_team1Unit;
    [SerializeField] private GameObject m_team2Unit;

    [Header("Level Spawn Points")]
    [SerializeField] private Transform[] m_teamOneSpawnPoints;
    [SerializeField] private Transform[] m_teamTwoSpawnPoints;

    private void Start()
    {
        SpawnInTeams();

        SpawnInPlayer();
    }

    private void SpawnInLevel()
    {

    }

    private void SpawnInTeams()
    {
        for (int teamOneUnit = 0; teamOneUnit < m_teamOneSpawnPoints.Length; teamOneUnit++)
        {
            GameObject unit = Instantiate(m_team1Unit, m_teamOneSpawnPoints[teamOneUnit].position, m_teamOneSpawnPoints[teamOneUnit].rotation);
            unit.GetComponent<UnitView>().SetTeam(1);
        }

        for (int teamTwoUnits = 0; teamTwoUnits < m_teamOneSpawnPoints.Length; teamTwoUnits++)
        {
            GameObject unit = Instantiate(m_team2Unit, m_teamTwoSpawnPoints[teamTwoUnits].position, m_teamTwoSpawnPoints[teamTwoUnits].rotation);
            unit.GetComponent<UnitView>().SetTeam(2);
        }
    }

    private void SpawnInPlayer()
    {
        Instantiate(m_playerPrefab, new Vector3(15, 10, 0), Quaternion.Euler(new Vector3(45, 0, 0)));
    }
}
