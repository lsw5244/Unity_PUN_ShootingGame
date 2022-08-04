using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private static NetworkManager _instance;
    public static NetworkManager Instance { get { return _instance; } }
    
    string gameVersion = "0.1";

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 네트워크 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Connect();
        PhotonNetwork.AutomaticallySyncScene = true;
        //Debug.Log(PhotonNetwork.IsConnected);
    }

    // Photon Server에 접속하는 함수
    void Connect()
    {
        Debug.Log("Connect호출 !!");
        if (PhotonNetwork.IsConnected == false)
        {
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.GameVersion = gameVersion;
            // 해당 버전으로 photon 클라이드로 연결되는 시작점 ( Photon Online Server에 접속하는 함수 )
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public bool EnterRoom(string RoomName)
    {
        //PhotonNetwork.LoadLevel(1);   // 마스터 클라이언트에서 호출하는 것

        if ( PhotonNetwork.JoinOrCreateRoom(RoomName, new RoomOptions { MaxPlayers = 2 }, null) )
        {
            return true;
        }

        return false;        
    }
}
