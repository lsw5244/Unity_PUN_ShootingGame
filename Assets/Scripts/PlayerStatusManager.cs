using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager Instance;

    public float maxHP = 100f;

    public float moveSpeed = 10f;
    public float jumpPower = 10f;

    public float fireDelay = 0.2f;
    public float bulletPower = 100f;

    public float attackDamage = 40f;

    public float explosionDamage = 20f;
    public float explosionRange = 1f;

    public float poisonDamage = 10f;
    public int poisonCount = 3;

    public int maxBulletCount = 3;

    public float reloadTime = 1.0f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning($"PlayerStatusManager 여러개가 감지되어 {gameObject.name}를 삭제합니다.");
            Destroy(gameObject);
        }
    }
}
