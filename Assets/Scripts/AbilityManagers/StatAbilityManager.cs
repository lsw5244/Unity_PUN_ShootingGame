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
        PlayerStatusManager.Instance.AttackDamage *= 2f;
        PlayerStatusManager.Instance.MaxHP /= 2f;
        PlayerStatusManager.Instance.ReloadTime += 0.25f;
    }

    public void Combine()
    {
        PlayerStatusManager.Instance.AttackDamage *= 2f;

        PlayerStatusManager.Instance.MaxBulletCount -= 2;
        if (PlayerStatusManager.Instance.MaxBulletCount <= 0)
        {
            PlayerStatusManager.Instance.MaxBulletCount = 1;
        }

        PlayerStatusManager.Instance.ReloadTime += 0.5f;        
    }

    //public void Sniper()
    //{
    //    PlayerStatusManager.Instance.BulletPower *= 2f;
    //}
}
