using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class HitAbilityManager : MonoBehaviour
{
    public static HitAbilityManager Instance;
    
    public delegate void HitAbilityDelegate(GameObject ShootPlayer, GameObject HitPlayer);
    public HitAbilityDelegate hitAbility;

    private PhotonView photonView;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            photonView = gameObject.AddComponent<PhotonView>();

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning($"HitAbilityManager �������� �����Ǿ� {gameObject.name}�� �����մϴ�.");
            Destroy(gameObject);
        }
    }

    public void AddPoisonBullet()
    {
        if(PlayerStatManager.Instance.PoisonDamage <= 0f)
        {
            hitAbility -= PoisonBullet;
            hitAbility += PoisonBullet;
            PlayerStatManager.Instance.PoisonCount = 3;
        }
        // ƽ�� �ߵ� �������� 10�� �����ϵ��� ����
        PlayerStatManager.Instance.PoisonDamage += 10f;
    }

    void PoisonBullet(GameObject ShootPlayer, GameObject HitPlayer)
    {        
        float poisonDamage = ShootPlayer.GetComponent<PlayerState>().poisonDamage;
        int poisonCount = ShootPlayer.GetComponent<PlayerState>().poisonCount;

        HitPlayer.GetComponent<IDebuff>().StartPoison(poisonDamage, poisonCount);
    }

    public void AddFreezeBullet()
    {
        if(PlayerStatManager.Instance.FreezeTime <= 0f)
        {
            hitAbility -= FreezeBullet;
            hitAbility += FreezeBullet;
        }

        PlayerStatManager.Instance.FreezeTime += 0.5f;
    }

    void FreezeBullet(GameObject ShootPlayer, GameObject HitPlayer)
    {
        HitPlayer.GetComponent<IDebuff>().StartMoveFreeze(ShootPlayer.GetComponent<PlayerState>().freezeTime);
    }
}
