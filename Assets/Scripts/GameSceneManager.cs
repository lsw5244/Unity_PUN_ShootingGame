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

    [SerializeField]
    private Image fadeImage;

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

    private void Start()
    {
        GameScoreManager.Instance.StartRound();
        StartCoroutine("FadeIn");
    }

    public void EndGame(PlayerType winner)
    {      
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

    public void LoadNextRound()
    {
        photonView.RPC("LoadNextRoundRPC", RpcTarget.All);
    }

    [PunRPC]
    void LoadNextRoundRPC()
    {
        PhotonNetwork.LoadLevel(2);
    }

    IEnumerator FadeIn() // 밝아지기
    {
        while(fadeImage.fillAmount > 0)
        {
            fadeImage.fillAmount -= 0.015f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
