using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        // 점수 2번 올리기 + UI 2번 활성화 방지
        if(GameScoreManager.Instance.GetPlayRound() == false)
        {
            return;
        }

        // 3점이 되었을 때 처리하기

        if(winner == PlayerType.Blue)
        {
            GameScoreManager.Instance.BluePlayerScoreUP();
            if(GameScoreManager.Instance.BluePlayerWinCheck() == true)
            {
                Debug.Log("Blue플레이어가 게임에서 승리했다 !!!");
                return;
            }

            photonView.RPC("AbilitySelectCanvasActivateRPC", RpcTarget.All, true);
        }
        else
        {
            GameScoreManager.Instance.PinkPlayerScoreUp();
            photonView.RPC("AbilitySelectCanvasActivateRPC", RpcTarget.All, false);
        }
    }

    [PunRPC]
    void AbilitySelectCanvasActivateRPC(bool bluePlayerWin = true)
    {
        StartCoroutine(AbilitySelectCanvasActivate(bluePlayerWin));
    }

    IEnumerator AbilitySelectCanvasActivate(bool bluePlayerWin = true)
    {
        // 페이드 아웃 효과
        fadeImage.fillAmount = 0;
        while (fadeImage.fillAmount < 1)
        {
            fadeImage.fillAmount += FadeProduction.FadeSpeed;
            yield return new WaitForSeconds(FadeProduction.FadeDelay);
        }

        yield return new WaitForSeconds(FadeProduction.NextActionDelay);

        AbilityUISetting(bluePlayerWin);
        
        // 페이드 인 효과
        fadeImage.fillAmount = 1;
        while (fadeImage.fillAmount > 0)
        {
            fadeImage.fillAmount -= FadeProduction.FadeSpeed;
            yield return new WaitForSeconds(FadeProduction.FadeDelay);
        }
    }

    void AbilityUISetting(bool bluePlayerWin = true)
    {
        abilitySelectCanvas.SetActive(true);
        abilityAdder.gameEnd = true;

        if (bluePlayerWin == true)
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
        photonView.RPC("ChangeSceneRPC", RpcTarget.All);
    }

    [PunRPC]
    void ChangeSceneRPC()
    {        
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        fadeImage.fillAmount = 0;
        while (fadeImage.fillAmount < 1)
        {
            fadeImage.fillAmount += FadeProduction.FadeSpeed;
            yield return new WaitForSeconds(FadeProduction.FadeDelay);
        }

        yield return new WaitForSeconds(FadeProduction.NextActionDelay);

        if(PhotonNetwork.IsMasterClient == true)
        {
            photonView.RPC("LoadSceneRPC", RpcTarget.All, 2);
        }
    }

    [PunRPC]
    void LoadSceneRPC(int sceneNumber)
    {
        PhotonNetwork.LoadLevel(sceneNumber);
    }

    IEnumerator FadeIn()
    {
        fadeImage.fillAmount = 1;
        while (fadeImage.fillAmount > 0)
        {
            fadeImage.fillAmount -= FadeProduction.FadeSpeed;
            yield return new WaitForSeconds(FadeProduction.FadeDelay);
        }
    }
}
