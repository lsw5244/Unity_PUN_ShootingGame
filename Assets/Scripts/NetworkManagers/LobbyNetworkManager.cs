using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text networkStateText;
    [SerializeField]
    private Button roomConnectBtn;
    [SerializeField]
    private Text roomNameText;

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

    public void TryToEnterRoom()
    {
        Debug.Log("EnterRoom!!!!");

        if (roomNameText.text == "")
            return;

        if (NetworkManager.Instance.EnterRoom(roomNameText.text) == true)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            networkStateText.text = "�濡 �������� ���߽��ϴ�.";
        }
    }
}
