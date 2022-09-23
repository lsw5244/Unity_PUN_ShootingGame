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
    private Image loserPlayerImage;
    [SerializeField]
    private Sprite bluePlayerSprite;
    [SerializeField]
    private Sprite pinkPlayerSprite;

    [SerializeField]
    private AbilityAdder abilityAdder;

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

    public void EndGame(PlayerType winner)
    {
         /*
            1. ���ھ� ����
            2. �����Ƽ ���� �ǳ� Ȱ��ȭ
         */
        
        if(winner == PlayerType.Blue)
        {
            GameScoreManager.Instance.BluePlayerScoreUP();
            Debug.Log("Blue�÷��̾ �¸��Ͽ� ������ �ö��� !!!");
            photonView.RPC("AbilitySelectCanvasSettingRPC", RpcTarget.All, true, true);
        }
        else
        {
            GameScoreManager.Instance.PinkPlayerScoreUp();
            Debug.Log("Pink�÷��̾ �¸��Ͽ� ������ �ö��� !!!");
            photonView.RPC("AbilitySelectCanvasSettingRPC", RpcTarget.All, true, false);
        }
    }

    [PunRPC]
    void AbilitySelectCanvasSettingRPC(bool canvasActive, bool bluePlayerWin = true)
    {
        abilitySelectCanvas.SetActive(true);

        abilityAdder.gameEnd = true;

        if(bluePlayerWin == true)
        {
            loserPlayerImage.sprite = pinkPlayerSprite;
            abilityAdder.winnerPlayer = PlayerType.Blue;
        }
        else
        {
            loserPlayerImage.sprite = bluePlayerSprite;
            abilityAdder.winnerPlayer = PlayerType.Pink;
        }
    }
}
