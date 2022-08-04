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
            Debug.LogWarning("���� �ΰ� �̻��� ��Ʈ��ũ �Ŵ����� �����մϴ�!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Connect();
        PhotonNetwork.AutomaticallySyncScene = true;
        //Debug.Log(PhotonNetwork.IsConnected);
    }

    // Photon Server�� �����ϴ� �Լ�
    void Connect()
    {
        Debug.Log("Connectȣ�� !!");
        if (PhotonNetwork.IsConnected == false)
        {
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.GameVersion = gameVersion;
            // �ش� �������� photon Ŭ���̵�� ����Ǵ� ������ ( Photon Online Server�� �����ϴ� �Լ� )
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public bool EnterRoom(string RoomName)
    {
        //PhotonNetwork.LoadLevel(1);   // ������ Ŭ���̾�Ʈ���� ȣ���ϴ� ��

        if ( PhotonNetwork.JoinOrCreateRoom(RoomName, new RoomOptions { MaxPlayers = 2 }, null) )
        {
            return true;
        }

        return false;        
    }
}
