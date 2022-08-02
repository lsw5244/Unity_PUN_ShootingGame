using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text networkStateText;
    [SerializeField]
    private Button roomConnectBtn;

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
