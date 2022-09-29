using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class RoomNetworkManager : MonoBehaviourPunCallbacks
{
    //private PhotonView photonView;

    [SerializeField]
    private GameObject matchingUI;
    [SerializeField]
    private GameObject matchingCompleteUI;
    [SerializeField]
    private GameObject gameWaitingUI;
    [SerializeField]
    private GameObject startGameUI;

    private bool serverReady = false;
    private bool clientReady = false;

    [SerializeField]
    private Image fadeImage;

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            matchingUI.SetActive(true);
        }
        else
        {
            photonView.RPC("MatchingComplete", RpcTarget.All);
        }
    }

    [PunRPC]
    void MatchingComplete()
    {
        matchingUI.SetActive(false);
        matchingCompleteUI.SetActive(true);
    }

    public void GameReady()
    {
        matchingCompleteUI.SetActive(false);
        gameWaitingUI.SetActive(true);

        if (PhotonNetwork.IsMasterClient == true)
        {
            serverReady = true;

            GameStartCheck();
        }
        else
        {
            photonView.RPC("ClientReady", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void ClientReady()
    {
        clientReady = true;
        GameStartCheck();
    }
    
    void GameStartCheck()
    {
        if( clientReady == true && serverReady == true)
        {
            photonView.RPC("ChangeSceneRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void ChangeSceneRPC()
    {
        gameWaitingUI.SetActive(false);
        startGameUI.SetActive(true);

        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.fillAmount = 0;

        while (fadeImage.fillAmount < 1)
        {
            fadeImage.fillAmount += FadeProduction.FadeSpeed;
            yield return new WaitForSeconds(FadeProduction.FadeDelay);
        }

        yield return new WaitForSeconds(FadeProduction.NextActionDelay);

        if (PhotonNetwork.IsMasterClient == true)
        {
            photonView.RPC("LoadSceneRPC", RpcTarget.All, 2);
        }
    }

    [PunRPC]
    void LoadSceneRPC(int sceneNumber)
    {
        PhotonNetwork.LoadLevel(2);
    }
}
