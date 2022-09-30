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
        // ���� 2�� �ø��� + UI 2�� Ȱ��ȭ ����
        if(GameScoreManager.Instance.GetPlayRound() == false)
        {
            return;
        }

        // 3���� �Ǿ��� �� ó���ϱ�

        if(winner == PlayerType.Blue)
        {
            GameScoreManager.Instance.BluePlayerScoreUP();
            if(GameScoreManager.Instance.BluePlayerWinCheck() == true)
            {
                Debug.Log("Blue�÷��̾ ���ӿ��� �¸��ߴ� !!!");
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
        // ���̵� �ƿ� ȿ��
        fadeImage.fillAmount = 0;
        while (fadeImage.fillAmount < 1)
        {
            fadeImage.fillAmount += FadeProduction.FadeSpeed;
            yield return new WaitForSeconds(FadeProduction.FadeDelay);
        }

        yield return new WaitForSeconds(FadeProduction.NextActionDelay);

        AbilityUISetting(bluePlayerWin);
        
        // ���̵� �� ȿ��
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
