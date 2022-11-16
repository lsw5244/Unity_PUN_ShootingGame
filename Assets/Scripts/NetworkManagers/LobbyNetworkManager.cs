using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text networkStateText;
    [SerializeField]
    private Button roomConnectBtn;
    [SerializeField]
    private Text roomNameText;

    string gameVersion = "0.1";

    private void Start()
    {
        if(PhotonNetwork.IsConnected == true)
        {
            OnJoinedLobby();
        }
        else
        {
            Connect();
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Photon Server에 접속하는 함수
    void Connect()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.GameVersion = gameVersion;
            // 해당 버전으로 photon 클라이드로 연결되는 시작점 ( Photon Online Server에 접속하는 함수 )
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void TryToEnterRoom()
    {
        if (roomNameText.text == "")
            return;

        // Room입장 시도 및 씬 변경
        if (EnterRoom(roomNameText.text) == true)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            networkStateText.text = "방에 입장하지 못했습니다.";
        }
    }

    bool EnterRoom(string RoomName)
    {
        return PhotonNetwork.JoinOrCreateRoom(RoomName, new RoomOptions { MaxPlayers = 2 }, null);
        //if (PhotonNetwork.JoinOrCreateRoom(RoomName, new RoomOptions { MaxPlayers = 2 }, null))
        //{
        //    return true;
        //}

        //return false;
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
