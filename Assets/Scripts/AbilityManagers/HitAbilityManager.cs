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

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            hitAbility -= PoisonBullet;
            hitAbility += PoisonBullet;
            Debug.Log("독 총알 추가");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            hitAbility -= PoisonBullet;
            Debug.Log("독 총알 제거");
        }
    }

    public void AddPoisonBullet()
    {
        if(PlayerStatusManager.Instance.PoisonDamage <= 0)
        {
            hitAbility -= PoisonBullet;
            hitAbility += PoisonBullet;
            PlayerStatusManager.Instance.PoisonCount = 3;
        }
        // 틱당 중독 데미지가 10씩 증가하도록 구현
        PlayerStatusManager.Instance.PoisonDamage += 10f;
    }

    void PoisonBullet(GameObject ShootPlayer, GameObject HitPlayer)
    {        
        float poisonDamage = ShootPlayer.GetComponent<PlayerState>().poisonDamage;
        int poisonCount = ShootPlayer.GetComponent<PlayerState>().poisonCount;

        HitPlayer.GetComponent<PlayerDebuff>().StartPoison(poisonDamage, poisonCount);
    }
}
