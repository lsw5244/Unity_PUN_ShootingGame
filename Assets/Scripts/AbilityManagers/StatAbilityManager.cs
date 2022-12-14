using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class StatAbilityManager : MonoBehaviour
{
    private static StatAbilityManager _instance;
    public static StatAbilityManager Instance { get { return _instance; } }

    private PhotonView photonView;

    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;

            photonView = gameObject.AddComponent<PhotonView>();

            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 GameScoreManager가 존재합니다!");
            Destroy(gameObject);
        }
    }

    public void GlassCannon()
    {
        PlayerStatManager.Instance.AttackDamage *= 2f;
        PlayerStatManager.Instance.MaxHP /= 2f;
        PlayerStatManager.Instance.ReloadTime += 0.25f;
    }

    public void Combine()
    {
        PlayerStatManager.Instance.AttackDamage *= 2f;

        PlayerStatManager.Instance.MaxBulletCount -= 2;
        if (PlayerStatManager.Instance.MaxBulletCount <= 0)
        {
            PlayerStatManager.Instance.MaxBulletCount = 1;
        }

        PlayerStatManager.Instance.ReloadTime += 0.5f;        
    }

    public void Sniper()
    {
        PlayerStatManager.Instance.BulletPower *= 1.5f;
        PlayerStatManager.Instance.FireDelay += 0.25f;
    }
}
