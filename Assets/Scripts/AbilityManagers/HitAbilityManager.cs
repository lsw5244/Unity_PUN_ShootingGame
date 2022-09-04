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
            Debug.LogWarning($"HitAbilityManager �������� �����Ǿ� {gameObject.name}�� �����մϴ�.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            hitAbility -= PoisonBullet;
            hitAbility += PoisonBullet;
            Debug.Log("���� �Ѿ� �߰�");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            hitAbility -= PoisonBullet;
            Debug.Log("���� �Ѿ� ����");
        }
    }

    void PoisonBullet(GameObject ShootPlayer, GameObject HitPlayer)
    {        
        float poisonDamage = ShootPlayer.GetComponent<PlayerState>().poisonDamage;
        int poisonCount = ShootPlayer.GetComponent<PlayerState>().poisonCount;

        HitPlayer.GetComponent<PlayerDebuff>().StartPoison(poisonDamage, poisonCount);
    }
}
