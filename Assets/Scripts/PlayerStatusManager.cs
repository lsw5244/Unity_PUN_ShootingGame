using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    public static PlayerStatusManager Instance;

    public float MaxHP { get; set; } = 100f;

    public float MoveSpeed { get; set; } = 7f;
    public float JumpPower { get; set; } = 400f;

    public float FireDelay { get; set; } = 0.2f;
    public float BulletPower { get; set; } = 750f;

    public float AttackDamage { get; set; } = 40f;

    public float ExplosionDamage { get; set; } = 20f;
    public float ExplosionRange { get; set; } = 1f;

    public float PoisonDamage { get; set; } = 10f;
    public int PoisonCount { get; set; } = 3;

    public int MaxBulletCount { get; set; } = 3;

    public float ReloadTime { get; set; } = 1.0f;

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
