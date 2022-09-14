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
        /* �ؾ��� �� �ߺ����� ���� 3�� �̱� */        
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
            Debug.Log($"{i}��° ���ڴ� {selectAbilityIdxs[i]}");
        }
    }
    /* ===========�Ʒ����� Ư�� �߰� �Լ�=========== */
    void AddBulletExplosion()
    {
        Debug.Log("AddBulletExplosion ���� !!!");
    }

    public void AddPoisonBullet()
    {
        Debug.Log("AddPoisonBullet ���� !!!");
    }

    public void AddGlassCannon()
    {
        Debug.Log("AddGlassCannon ���� !!!");
    }

    public void AddCombine()
    {
        Debug.Log("AddCombine ���� !!!");
    }
}
