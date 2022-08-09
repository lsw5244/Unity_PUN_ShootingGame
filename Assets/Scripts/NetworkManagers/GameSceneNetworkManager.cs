using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class GameSceneNetworkManager : MonoBehaviour
{
    [SerializeField]
    private Transform lSpawnPos;
    [SerializeField]
    private Transform rSpawnPos;

    private void Awake()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            PhotonNetwork.Instantiate("OrangePlayer", lSpawnPos.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("OrangePlayer", rSpawnPos.position, Quaternion.identity);
        }
    }
}
