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
        /* �ߺ����� ���� 3�� �̱� */        
        ChooseAbilityIdxs();

        // ���÷����� ���� ��Ÿ�� �� ���ڿ��� �Լ� �����Ű��
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

                Debug.Log($"���� ���� idx {currentSelectAbilityIdx}");
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                currentSelectAbilityIdx = Mathf.Min(2, ++currentSelectAbilityIdx);
                Debug.Log($"���� ���� idx {currentSelectAbilityIdx}");
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
            Debug.Log($"{i}��° ���ڴ� {selectAbilityIdxs[i]}");
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
