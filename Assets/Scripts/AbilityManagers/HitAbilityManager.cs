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
            Debug.LogWarning($"HitAbilityManager 여러개가 감지되어 {gameObject.name}를 삭제합니다.");
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
        // 틱당 중독 데미지가 10씩 증가하도록 구현
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
