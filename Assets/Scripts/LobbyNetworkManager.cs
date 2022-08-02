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

    // Photon Server�� ����
    void Connect()
    {
        if (PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            // �ش� �������� photon Ŭ���̵�� ����Ǵ� ������ ( Photon Online Server�� �����ϴ� �Լ� )
            PhotonNetwork.ConnectUsingSettings();
        }
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
