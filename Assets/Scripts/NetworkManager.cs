using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    string gameVersion = "0.1";

    private void Start()
    {
        Connect();
        //Debug.Log(PhotonNetwork.IsConnected);
    }

    // Photon Server에 접속하는 함수
    void Connect()
    {
        Debug.Log("Connect호출 !!");
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
}
