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

    public void EndGame(PlayerType winner)
    {
        /*
            1. 스코어 증가
            2. 어빌리티 선택 판넬 활성화
         */
        
        if(winner == PlayerType.Blue)
        {
            GameScoreManager.Instance.BluePlayerScoreUP();
            Debug.Log("Blue플레이어가 승리하여 점수가 올랐다 !!!");
            photonView.RPC("AbilitySelectCanvasSettingRPC", RpcTarget.All, true, true);
        }
        else
        {
            GameScoreManager.Instance.PinkPlayerScoreUp();
            Debug.Log("Pink플레이어가 승리하여 점수가 올랐다 !!!");
            photonView.RPC("AbilitySelectCanvasSettingRPC", RpcTarget.All, true, false);
        }
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
