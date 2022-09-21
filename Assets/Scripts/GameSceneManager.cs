using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class GameSceneManager : MonoBehaviour
{
    private PhotonView photonView;

    [SerializeField]
    private Transform lSpawnPos;
    [SerializeField]
    private Transform rSpawnPos;

    [SerializeField]
    private string lPlayerPrefabName;
    [SerializeField]
    private string rPlayerPrefabName;

    [SerializeField]
    private GameObject abilitySelectCanvas;

    private GameObject myPlayer;

    private void Awake()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            myPlayer = PhotonNetwork.Instantiate(lPlayerPrefabName, lSpawnPos.position, Quaternion.identity);
        }
        else
        {
            myPlayer = PhotonNetwork.Instantiate(rPlayerPrefabName, rSpawnPos.position, Quaternion.identity);            
        }

        photonView = GetComponent<PhotonView>();
    }


    public void EndGame(bool leftPlayerWin)
    {
        /*
            1. ���ھ� ����
            2. �����Ƽ ���� �ǳ� Ȱ��ȭ
         */

        if(leftPlayerWin == true)
        {
            GameScoreManager.Instance.LeftPlayerScoreUP();
        }
        else
        {
            GameScoreManager.Instance.RightPlayerScoreUp();
        }

        photonView.RPC("AbilitySelectCanvasSetActiveRPC", RpcTarget.All, true);
    }

    [PunRPC]
    void AbilitySelectCanvasSetActiveRPC(bool active)
    {
        abilitySelectCanvas.SetActive(true);
    }
}
