using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using Photon.Pun;

public class RoundManager : MonoBehaviour
{
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            if(Input.GetKeyDown(KeyCode.Keypad9))
            {
                photonView.RPC("ChangeNextRound", RpcTarget.All);
            }

            if(Input.GetKeyDown(KeyCode.Keypad8))
            {
                StatAbilityManager.Instance.Combine();
            }
        }
    }

    [PunRPC]
    void ChangeNextRound()
    {
        PhotonNetwork.LoadLevel(2);
    }
}
