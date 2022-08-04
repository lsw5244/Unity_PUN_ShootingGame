using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class RoomNetworkManager : MonoBehaviourPunCallbacks
{
    //private PhotonView photonView;

    [SerializeField]
    private GameObject matchingUI;
    [SerializeField]
    private GameObject matchingCompleteUI;
    [SerializeField]
    private GameObject gameWaitingUI;

    [SerializeField]
    private bool serverReady = false;
    [SerializeField]
    private bool clientReady = false;

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            matchingUI.SetActive(true);
        }
        else
        {
            photonView.RPC("MatchingComplete", RpcTarget.All);
        }
    }

    [PunRPC]
    void MatchingComplete()
    {
        matchingUI.SetActive(false);
        matchingCompleteUI.SetActive(true);
    }

    public void GameReady()
    {
        matchingCompleteUI.SetActive(false);
        gameWaitingUI.SetActive(true);

        if (PhotonNetwork.IsMasterClient == true)
        {
            serverReady = true;

            GameStartCheck();
        }
        else
        {
            photonView.RPC("ClientReady", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void ClientReady()
    {
        clientReady = true;
        GameStartCheck();
    }
    
    void GameStartCheck()
    {
        if( clientReady == true && serverReady == true)
        {
            PhotonNetwork.LoadLevel(2);
        }
    }
}
