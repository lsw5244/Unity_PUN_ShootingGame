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

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            hitAbility -= PoisonBullet;
            hitAbility += PoisonBullet;
            Debug.Log("�� �Ѿ� �߰�");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            hitAbility -= PoisonBullet;
            Debug.Log("�� �Ѿ� ����");
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
        // ƽ�� �ߵ� �������� 10�� �����ϵ��� ����
        PlayerStatusManager.Instance.PoisonDamage += 10f;
    }

    void PoisonBullet(GameObject ShootPlayer, GameObject HitPlayer)
    {        
        float poisonDamage = ShootPlayer.GetComponent<PlayerState>().poisonDamage;
        int poisonCount = ShootPlayer.GetComponent<PlayerState>().poisonCount;

        HitPlayer.GetComponent<PlayerDebuff>().StartPoison(poisonDamage, poisonCount);
    }
}
