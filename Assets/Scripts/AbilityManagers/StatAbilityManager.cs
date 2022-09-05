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
            Debug.LogWarning("���� �ΰ� �̻��� GameScoreManager�� �����մϴ�!");
            Destroy(gameObject);
        }
    }

    public void GlassCannon()
    {

    }

    public void Combine()
    {
        //PlayerStatusManager.Instance.AttackDamage *= 3;

        //PlayerStatusManager.Instance.MaxBulletCount -= 2;
        //if(PlayerStatusManager.Instance.MaxBulletCount <= 0)
        //{
        //    PlayerStatusManager.Instance.MaxBulletCount = 1;
        //}

        //PlayerStatusManager.Instance.FireDelay += 0.3f;
    }
}
