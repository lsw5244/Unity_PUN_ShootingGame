using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
using Photon.Pun;

public class AbilityAdder : MonoBehaviour
{
    private string[] addAbilityNames
        = { "AddBulletExplosion", "AddPoisonBullet", "AddGlassCannon", "AddCombine"  };

    private int[] selectAbilityIdxs = new int[3];
    private int currentSelectAbilityIdx = 0;

    private System.Type type;

    private void Start()
    {
        /* 중복없이 숫자 3개 뽑기 */        
        ChooseAbilityIdxs();

        // 리플렉션을 통해 런타임 중 문자열로 함수 실행시키기
        type = this.GetType();
        MethodInfo mi = type.GetMethod("AddBulletExplosion", BindingFlags.NonPublic | BindingFlags.Instance);

        //mi.Invoke(this, null);
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
                MethodInfo mi = type.GetMethod(addAbilityNames[selectAbilityIdxs[currentSelectAbilityIdx]], BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(this, null);
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {

            Debug.Log(type);

        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            MethodInfo mi = type.GetMethod(addAbilityNames[0], BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(this, null);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MethodInfo mi = type.GetMethod(addAbilityNames[1], BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(this, null);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MethodInfo mi = type.GetMethod(addAbilityNames[2], BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(this, null);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MethodInfo mi = type.GetMethod(addAbilityNames[3], BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(this, null);
        }
    }     

    void ChooseAbilityIdxs()
    {
        for(int i = 0; i < selectAbilityIdxs.Length; ++i)
        {
            selectAbilityIdxs[i] = Random.Range(0, addAbilityNames.Length);
            
            for(int j = 0; j < i; j++)
            {
                if (selectAbilityIdxs[i] == selectAbilityIdxs[j])
                {
                    i--;
                    break;
                }
            }
        }

        for(int i = 0; i < selectAbilityIdxs.Length; ++i)
        {
            Debug.Log($"{i}번째 숫자는 {selectAbilityIdxs[i]}");
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
