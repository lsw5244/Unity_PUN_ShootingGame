using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Reflection;
using Photon.Pun;

public class AbilityAdder : MonoBehaviour
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

    private void Start()
    {
        // �����Ͱ� ���ϰ� �й��Ű��
        /* �ߺ����� ���� 3�� �̱� */
        ChooseAbilityIdxs();

        for (int i = 0; i < randomAbilityIdxs.Length; ++i)
        {
            abilityImages[i].sprite = abilityImagesResources[randomAbilityIdxs[i]];
            abilityNameTexts[i].text = addAbilityNames[randomAbilityIdxs[i]].Replace("Add", "");
        }
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                //currentSelectAbilityIdx--;
                currentSelectAbilityIdx = Mathf.Max(0, --currentSelectAbilityIdx);

                Debug.Log($"���� ���� idx {currentSelectAbilityIdx}");
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                currentSelectAbilityIdx = Mathf.Min(2, ++currentSelectAbilityIdx);
                Debug.Log($"���� ���� idx {currentSelectAbilityIdx}");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                MethodInfo mi = type.GetMethod(addAbilityNames[randomAbilityIdxs[currentSelectAbilityIdx]], BindingFlags.NonPublic | BindingFlags.Instance);
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

        for(int i = 0; i < randomAbilityIdxs.Length; ++i)
        {
            Debug.Log($"{i}��° ���ڴ� {randomAbilityIdxs[i]}");
        }
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
