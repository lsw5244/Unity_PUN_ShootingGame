using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Image winnerImage;
    [SerializeField]
    private Sprite bluePlayerSprite;
    [SerializeField]
    private Sprite pinkPlayerSprite;

    private void Awake()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            PhotonNetwork.Instantiate(lPlayerPrefabName, lSpawnPos.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(rPlayerPrefabName, rSpawnPos.position, Quaternion.identity);            
        }

        photonView = GetComponent<PhotonView>();
    }

    public void EndGame(bool bluePlayerWin)
    {
        /*
            1. ���ھ� ����
            2. �����Ƽ ���� �ǳ� Ȱ��ȭ
         */

        PlayerType t = PlayerType.Blue;

        if (bluePlayerWin == true)
        {
            GameScoreManager.Instance.BluePlayerScoreUP();
            Debug.Log("Blue�÷��̾ �¸��Ͽ� ������ �ö��� !!!");
        }
        else
        {
            GameScoreManager.Instance.PinkPlayerScoreUp();
            Debug.Log("Pink�÷��̾ �¸��Ͽ� ������ �ö��� !!!");
        }

        photonView.RPC("AbilitySelectCanvasSettingRPC", RpcTarget.All, true, bluePlayerWin);
    }

    [PunRPC]
    void AbilitySelectCanvasSettingRPC(bool canvasActive, bool bluePlayerWin = true)
    {
        abilitySelectCanvas.SetActive(true);

        if(bluePlayerWin == true)
        {
            winnerImage.sprite = bluePlayerSprite;
        }
        else
        {
            winnerImage.sprite = pinkPlayerSprite;
        }
    }
}