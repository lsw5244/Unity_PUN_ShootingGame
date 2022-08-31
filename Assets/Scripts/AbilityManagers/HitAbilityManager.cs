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
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hitAbility -= PoisonBullet;
            hitAbility += PoisonBullet;
        }
    }

    void PoisonBullet(GameObject ShootPlayer, GameObject HitPlayer)
    {
        float poisonDamage = ShootPlayer.GetComponent<PlayerState>().poisonDamage;
        int poisonCount = ShootPlayer.GetComponent<PlayerState>().poisonCount;

        HitPlayer.GetComponent<PlayerDebuff>().StartPoison(poisonDamage, poisonCount);
    }
}
