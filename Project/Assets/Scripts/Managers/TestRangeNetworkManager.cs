using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class TestRangeNetworkManager : NetworkManager
{
    [Header("Test Range Spawn Points")]    
    [SerializeField]
    private Transform[] playerOneUnitSpawnPoints;
    [SerializeField]
    private Transform[] playerTwoUnitSpawnPoints;

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        JoinOrCreateRoom();       
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom(); 

        if(IsRoomFull())
        {
            SpawnInPlayerTwo();
        }
        else
        {
            SpawnInPlayerOne();
        }
    }

    private void SpawnInPlayerOne()
    {
        Vector3 spawnPosition = new Vector3(0, 10, -15);
        Vector3 spawnRotation = new Vector3(45, 0, 0);
        PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.Euler(spawnRotation),0);

        for (int unitSpawnPointIndex = 0; unitSpawnPointIndex < playerOneUnitSpawnPoints.Length; unitSpawnPointIndex++)
        {
            spawnPosition = playerOneUnitSpawnPoints[unitSpawnPointIndex].position;
            Quaternion spawnQuaternion = playerOneUnitSpawnPoints[unitSpawnPointIndex].rotation;

            PhotonNetwork.Instantiate("Unit", spawnPosition, spawnQuaternion, 0);
        }
    }

    private void SpawnInPlayerTwo()
    {
        Vector3 spawnPosition = new Vector3(0, 10, 15);
        Vector3 spawnRotation = new Vector3(45,180, 0);
        PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.Euler(spawnRotation), 0);

        for (int unitSpawnPointIndex = 0; unitSpawnPointIndex < playerTwoUnitSpawnPoints.Length; unitSpawnPointIndex++)
        {
            spawnPosition = playerTwoUnitSpawnPoints[unitSpawnPointIndex].position;
            Quaternion spawnQuaternion = playerTwoUnitSpawnPoints[unitSpawnPointIndex].rotation;

            PhotonNetwork.Instantiate("Unit", spawnPosition, spawnQuaternion, 0);
        }
    }


}

