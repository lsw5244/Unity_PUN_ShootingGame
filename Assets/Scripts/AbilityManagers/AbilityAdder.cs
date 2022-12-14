using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Reflection;
using Photon.Pun;

public class AbilityAdder : MonoBehaviour, IPunObservable
{
    private string[] addAbilityNames
        = { "AddBulletExplosion", "AddPoisonBullet", "AddGlassCannon", "AddCombine", "AddFreezeBullet"
            , "AddSniper"};
    private string[] abilityInfos
        = { "총알이 충격을 받으면 폭발합니다.", "총알에 독 데미지가 추가됩니다."
            , "데미지 X 2\nHp / 2\n재장전 시간 + 0.25s", "데미지 X2\n최대 장탄수 - 2\n재장전 시간 + 0.5s"
            , "총알을 맞은 상대는 이동불가 상태가 됩니다.", "총알 발사 파워 * 1.5\n발사 딜레이 + 0.25s"};

    [SerializeField]
    private Sprite[] abilityImagesResources;

    private int[] randomAbilityIdxs = new int[3];
    private int currentSelectAbilityIdx = 0;

    private System.Type type;

    [SerializeField]
    private Image[] abilityImages = new Image[3];
    [SerializeField]
    private Text[] abilityNameTexts = new Text[3];
    [SerializeField]
    private Text[] abilityInfoTexts = new Text[3];
    [SerializeField]
    private Image[] abilityCardOutLine = new Image[3];

    private PhotonView photonView;

    [HideInInspector]
    public bool gameEnd = false;
    private PlayerType myPlayerType;
    [HideInInspector]
    public PlayerType winnerPlayer;

    public GameSceneManager gameSceneManager;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 소유자가 다른사람에게 데이터 보내기
            for (int i = 0; i < randomAbilityIdxs.Length; ++i)
            {
                stream.SendNext(randomAbilityIdxs[i]);
            }
        }
        else
        {
            // 다른 클라이언트가 데이터 받기
            for (int i = 0; i < randomAbilityIdxs.Length; ++i)
            {
                this.randomAbilityIdxs[i] = (int)stream.ReceiveNext();
            }
            AbilityUiSetting();
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            ChooseAbilityIdxs();
            AbilityUiSetting();
            myPlayerType = PlayerType.Blue;
        }
        else
        {
            myPlayerType = PlayerType.Pink;
        }

        photonView = GetComponent<PhotonView>();
        photonView.RPC("CardOutLineActive", RpcTarget.All, currentSelectAbilityIdx, true);
    }

    private void Update()
    {
        if(myPlayerType != winnerPlayer && gameEnd == true)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                photonView.RPC("CardOutLineActive", RpcTarget.All, currentSelectAbilityIdx, false);
                currentSelectAbilityIdx = Mathf.Max(0, --currentSelectAbilityIdx);
                photonView.RPC("CardOutLineActive", RpcTarget.All, currentSelectAbilityIdx, true);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                photonView.RPC("CardOutLineActive", RpcTarget.All, currentSelectAbilityIdx, false);
                currentSelectAbilityIdx = Mathf.Min(2, ++currentSelectAbilityIdx);
                photonView.RPC("CardOutLineActive", RpcTarget.All, currentSelectAbilityIdx, true);            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //string selectAbilityName = addAbilityNames[randomAbilityIdxs[currentSelectAbilityIdx]];
                int selectAbilityIdx = randomAbilityIdxs[currentSelectAbilityIdx];
                string selectAbilityName = addAbilityNames[selectAbilityIdx];
                System.Type type = this.GetType();

                MethodInfo mi = type.GetMethod(selectAbilityName, BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(this, null);

                gameSceneManager.LoadNextRound();
                this.enabled = false;
            }
        }
    }     

    void ChooseAbilityIdxs()
    {
        for(int i = 0; i < randomAbilityIdxs.Length; ++i)
        {
            randomAbilityIdxs[i] = Random.Range(0, addAbilityNames.Length);
            
            for(int j = 0; j < i; j++)
            {
                if (randomAbilityIdxs[i] == randomAbilityIdxs[j])
                {
                    i--;
                    break;
                }
            }
        }
    }

    void AbilityUiSetting()
    {
        for (int i = 0; i < randomAbilityIdxs.Length; ++i)
        {
            abilityImages[i].sprite = abilityImagesResources[randomAbilityIdxs[i]];
            abilityNameTexts[i].text = addAbilityNames[randomAbilityIdxs[i]].Replace("Add", "");
            abilityInfoTexts[i].text = abilityInfos[randomAbilityIdxs[i]];
        }
    }

    [PunRPC]
    void CardOutLineActive(int outLineidx, bool active)
    {
        abilityCardOutLine[outLineidx].gameObject.SetActive(active);
    }

    /* ===========아래에는 특성 추가 함수=========== */
    void AddBulletExplosion()
    {
        Debug.Log("0. AddBulletExplosion 실행 !!!");
        ImpactAbilityManager.Instance.AddBulletExplosion();
    }

    void AddPoisonBullet()
    {
        Debug.Log("1. AddPoisonBullet 실행 !!!");
        HitAbilityManager.Instance.AddPoisonBullet();
    }

    void AddGlassCannon()
    {
        Debug.Log("2. AddGlassCannon 실행 !!!");
        StatAbilityManager.Instance.GlassCannon();
    }

    void AddCombine()
    {
        Debug.Log("3. AddCombine 실행 !!!");
        StatAbilityManager.Instance.Combine();
    }

    void AddFreezeBullet()
    {
        Debug.Log("4. AddFreezeBullet 실행 !!!");
        HitAbilityManager.Instance.AddFreezeBullet();
    }

    void AddSniper()
    {
        Debug.Log("5. AddSniper 실행 !!!");
        StatAbilityManager.Instance.Sniper();
    }
}
