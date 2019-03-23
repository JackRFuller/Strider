using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkManager : Photon.MonoBehaviour
{
    [SerializeField]
    protected int requiredNumberOfPlayers = 2;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("Pre-Alpha");
        Debug.Log("ConnectedToMaster");
    }

    public virtual void OnConnectedToMaster()
    {
       
    }

    public virtual void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("newRoom", roomOptions, null);
    }

    public virtual void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
    }

    public virtual void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("Second Player Connected");
    }

    public bool IsRoomFull()
    {
        if (PhotonNetwork.playerList.Length == requiredNumberOfPlayers)
            return true;
        else
            return false;
    }
}
