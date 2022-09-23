using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Reflection;
using Photon.Pun;

public class AbilityAdder : MonoBehaviour, IPunObservable
{
    private string[] addAbilityNames
        = { "AddBulletExplosion", "AddPoisonBullet", "AddGlassCannon", "AddCombine"  };

    private string[] abilityInfos
        = { "�Ѿ��� ����� ������ �����մϴ�.", "�Ѿ˿� �� �������� �߰��˴ϴ�."
            , "������ X 2\nHp / 2\n������ �ð� + 0.25s", "������ X2\n�ִ� ��ź�� - 2\n������ �ð� + 0.5s" };
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

    public bool gameEnd = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����ڰ� �ٸ�������� ������ ������
            for (int i = 0; i < randomAbilityIdxs.Length; ++i)
            {
                stream.SendNext(randomAbilityIdxs[i]);
            }
        }
        else
        {
            // �ٸ� Ŭ���̾�Ʈ�� ������ �ޱ�
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
        }

        photonView = GetComponent<PhotonView>();
        photonView.RPC("CardOutLineActive", RpcTarget.All, currentSelectAbilityIdx, true);
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient == true && gameEnd == true)
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
                string selectAbilityName = addAbilityNames[randomAbilityIdxs[currentSelectAbilityIdx]];
                System.Type type = this.GetType();

                MethodInfo mi = type.GetMethod(selectAbilityName, BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(this, null);
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

    /* ===========�Ʒ����� Ư�� �߰� �Լ�=========== */
    void AddBulletExplosion()
    {
        Debug.Log("0. AddBulletExplosion ���� !!!");
        ImpactAbilityManager.Instance.AddBulletExplosion();
    }

    void AddPoisonBullet()
    {
        Debug.Log("1. AddPoisonBullet ���� !!!");
        HitAbilityManager.Instance.AddPoisonBullet();
    }

    void AddGlassCannon()
    {
        Debug.Log("2. AddGlassCannon ���� !!!");
        StatAbilityManager.Instance.GlassCannon();
    }

    void AddCombine()
    {
        Debug.Log("3. AddCombine ���� !!!");
        StatAbilityManager.Instance.Combine();
    }
}
