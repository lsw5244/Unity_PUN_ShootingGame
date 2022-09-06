using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager Instance;

    public float MaxHP { get; set; }

    public float MoveSpeed { get; set; }
    public float JumpPower { get; set; }

    public float FireDelay { get; set; }
    public float BulletPower { get; set; }

    public float AttackDamage { get; set; }

    public float ExplosionDamage { get; set; }
    public float ExplosionRange { get; set; }

    public float PoisonDamage { get; set; }
    public int PoisonCount { get; set; }

    public int MaxBulletCount { get; set; }

    public float ReloadTime { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitBasicStat();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning($"PlayerStatusManager �������� �����Ǿ� {gameObject.name}�� �����մϴ�.");
            Destroy(gameObject);
        }
    }

    void InitBasicStat()
    {
        MaxHP = 100f;

        MoveSpeed = 7f;
        JumpPower = 400f;
                
        FireDelay = 0.2f;
        BulletPower = 750f;

        AttackDamage = 40f;

        ExplosionDamage = 20f;
        ExplosionRange = 1f;

        PoisonDamage = 10f;
        PoisonCount = 3;

        MaxBulletCount = 3;

        ReloadTime  = 1.0f;
    }
}