using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAdder : MonoBehaviour
{
    private string[] addAbilityNames
        = { "AddBulletExplosion", "AddPoisonBullet", "AddGlassCannon", "AddCombine"  };

    private int[] selectAbilityIdxs = new int[3];

    private void Start()
    {
        /* 해야할 것 중복없이 숫자 3개 뽑기 */        
        ChooseAbilityIdxs();
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

        Random.Range(0, addAbilityNames.Length);
    }

    void AddBulletExplosion()
    {

    }

    void AddPoisonBullet()
    {

    }

    void AddGlassCannon()
    {

    }

    void AddCombine()
    {

    }
}
