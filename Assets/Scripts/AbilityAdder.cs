using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;

public class AbilityAdder : MonoBehaviour
{
    private string[] addAbilityNames
        = { "AddBulletExplosion", "AddPoisonBullet", "AddGlassCannon", "AddCombine"  };

    private int[] selectAbilityIdxs = new int[3];

    private System.Type type;

    private void Start()
    {
        /* 해야할 것 중복없이 숫자 3개 뽑기 */        
        ChooseAbilityIdxs();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            type = this.GetType();
            Debug.Log(type);
            MethodInfo mi = type.GetMethod("AddBulletExplosion", BindingFlags.NonPublic | BindingFlags.Instance);

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
        Debug.Log("AddBulletExplosion 실행 !!!");
    }

    public void AddPoisonBullet()
    {
        Debug.Log("AddPoisonBullet 실행 !!!");
    }

    public void AddGlassCannon()
    {
        Debug.Log("AddGlassCannon 실행 !!!");
    }

    public void AddCombine()
    {
        Debug.Log("AddCombine 실행 !!!");
    }
}
