using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    string gameVersion = "0.1";

    [SerializeField]
    private Text networkStateText;
    [SerializeField]
    private Button roomConnectBtn;

    private void Start()
    {
        Connect();
    }

    // Photon Server에 접속
    void Connect()
    {
        if (PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            // 해당 버전으로 photon 클라이드로 연결되는 시작점 ( Photon Online Server에 접속하는 함수 )
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        networkStateText.text = "서버에 접속되었습니다.";
        // Lobby에 접속
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        networkStateText.text = "로비에 입장하였습니다.";
        roomConnectBtn.interactable = true;
    }
}
