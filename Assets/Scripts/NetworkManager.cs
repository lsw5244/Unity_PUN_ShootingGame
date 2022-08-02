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
