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
        //Debug.Log(PhotonNetwork.IsConnected);
    }

    // Photon Server�� �����ϴ� �Լ�
    void Connect()
    {
        Debug.Log("Connectȣ�� !!");
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
}
