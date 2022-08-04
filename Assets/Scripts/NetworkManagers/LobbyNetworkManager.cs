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
        networkStateText.text = "서버에 접속되었습니다.";
        // Lobby에 접속
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        networkStateText.text = "로비에 입장하였습니다.";
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
            networkStateText.text = "방에 입장하지 못했습니다.";
        }
    }
}
