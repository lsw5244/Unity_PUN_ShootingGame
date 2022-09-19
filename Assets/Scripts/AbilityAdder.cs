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
        = { "총알이 충격을 받으면 폭발합니다.", "총알에 독 데미지가 추가됩니다."
            , "데미지 X 2\nHp / 2\n재장전 시간 + 0.25s", "데미지 X2\n최대 장탄수 - 2\n재장전 시간 + 0.5s" };
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
        // 마스터가 정하고 분배시키기
        /* 중복없이 숫자 3개 뽑기 */
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

                Debug.Log($"현재 선택 idx {currentSelectAbilityIdx}");
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                currentSelectAbilityIdx = Mathf.Min(2, ++currentSelectAbilityIdx);
                Debug.Log($"현재 선택 idx {currentSelectAbilityIdx}");
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
            Debug.Log($"{i}번째 숫자는 {randomAbilityIdxs[i]}");
        }
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
}
