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

    // Photon Server�� �����ϴ� �Լ�
    void Connect()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.GameVersion = gameVersion;
            // �ش� �������� photon Ŭ���̵�� ����Ǵ� ������ ( Photon Online Server�� �����ϴ� �Լ� )
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void TryToEnterRoom()
    {
        if (roomNameText.text == "")
            return;

        // Room���� �õ� �� �� ����
        if (EnterRoom(roomNameText.text) == true)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            networkStateText.text = "�濡 �������� ���߽��ϴ�.";
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
        networkStateText.text = "������ ���ӵǾ����ϴ�.";
        // Lobby�� ����
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        networkStateText.text = "�κ� �����Ͽ����ϴ�.";
        roomConnectBtn.interactable = true;
    }

}
