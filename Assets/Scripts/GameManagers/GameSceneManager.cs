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
    private GameObject[] maps;
    [SerializeField]
    private Transform[] lSpawnPos;
    [SerializeField]
    private Transform[] rSpawnPos;

    [SerializeField]
    private string lPlayerPrefabName;
    [SerializeField]
    private string rPlayerPrefabName;

    [SerializeField]
    private GameObject abilitySelectCanvas;
    [SerializeField]
    private GameObject gameOverCanvas;

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

    [HideInInspector]
    public bool playRound = true;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if(PhotonNetwork.IsMasterClient == true)
        {
            int selectMapIdx = Random.Range(0, maps.Length);
            photonView.RPC("RoundSetting",RpcTarget.All, selectMapIdx);
        }
    }

    [PunRPC]
    void RoundSetting(int mapIdx)
    {
        maps[mapIdx].SetActive(true);
        if (PhotonNetwork.IsMasterClient == true)
        {
            PhotonNetwork.Instantiate(lPlayerPrefabName, lSpawnPos[mapIdx].position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(rPlayerPrefabName, rSpawnPos[mapIdx].position, Quaternion.identity);
        }
    }

    private void Start()
    {
        playRound = true;
        StartCoroutine(FadeIn());
    }

    public void EndGame(PlayerType winner)
    {
        // ���� 2�� �ø��� + UI 2�� Ȱ��ȭ ����
        if(playRound == false)
        {
            return;
        }

        if(winner == PlayerType.Blue)
        {
            GameScoreManager.Instance.BluePlayerScoreUP();
            // 3�� �̻�(���� �¸�)�� �� ó��
            if(GameScoreManager.Instance.BluePlayerWinCheck() == true)
            {
                photonView.RPC("GameOverCanvasActivate", RpcTarget.All
                    , GameScoreManager.Instance.bluePlayerScore, GameScoreManager.Instance.pinkPlayerScore);
                return;
            }

            photonView.RPC("AbilitySelectCanvasActivateRPC", RpcTarget.All, true);
        }
        else
        {
            GameScoreManager.Instance.PinkPlayerScoreUp();
            if (GameScoreManager.Instance.PinkPlayerWinCheck() == true)
            {
                photonView.RPC("GameOverCanvasActivate", RpcTarget.All
                    , GameScoreManager.Instance.bluePlayerScore, GameScoreManager.Instance.pinkPlayerScore);
                return;
            }

            photonView.RPC("AbilitySelectCanvasActivateRPC", RpcTarget.All, false);
        }
    }

    [PunRPC]
    void GameOverCanvasActivate(int bluePlayerScore, int pinkPlayerscore)
    {
        gameOverCanvas.SetActive(true);
        gameOverCanvas.GetComponent<GameOverCanvas>().CanvasSetting(bluePlayerScore, pinkPlayerscore);
        PhotonNetwork.LeaveRoom();
        
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
        // AbilityUI Ȱ��ȭ
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
    // �ܺο��� ChangeSceneRPC�� ȣ���ϱ� ���� ���� �Լ�
    public void LoadNextRound()
    {
        photonView.RPC("ChangeSceneRPC", RpcTarget.All);
    }

    [PunRPC]
    void ChangeSceneRPC()
    {
        // ���̵��� ȿ�� �� �� ���� �� �ٷ� ���̵��� �ڷ�ƾ ����
        StopAllCoroutines();
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        // ���̵� �ƿ� ȿ��
        fadeImage.fillAmount = 0;
        while (fadeImage.fillAmount < 1)
        {
            fadeImage.fillAmount += FadeProduction.FadeSpeed;
            yield return new WaitForSeconds(FadeProduction.FadeDelay);
        }
        yield return new WaitForSeconds(FadeProduction.NextActionDelay);

        // ������ �� ����
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

    public void ExitGame()
    {
        DestoryManagerObjects();
        SceneManager.LoadScene(0);
    }

    void DestoryManagerObjects()
    {
        Destroy(GameObject.Find("GameScoreManager"));
        Destroy(GameObject.Find("ImpactAbilityManager"));
        Destroy(GameObject.Find("HitAbilityManager"));
        Destroy(GameObject.Find("StatAbilityManager"));
        Destroy(GameObject.Find("PlayerStatusManager"));
    }
}
