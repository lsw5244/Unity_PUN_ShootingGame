using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class ImpactAbilityManager : MonoBehaviour
{
    public static ImpactAbilityManager Instance;

    public delegate void ImpactAbilityDelegate(GameObject Player, Vector3 BulletPos);
    public ImpactAbilityDelegate impactAbility;

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
            Debug.LogWarning($"ImpactAbility �������� �����Ǿ� {gameObject.name}�� �����մϴ�.");
            Destroy(gameObject);
        }
    }

    public void AddBulletExplosion()
    {
        if(PlayerStatManager.Instance.ExplosionDamage <= 0f)
        {
            impactAbility -= BulletExplosion;
            impactAbility += BulletExplosion;
            PlayerStatManager.Instance.ExplosionRange = 1f;
        }
        // ���� �������� 20�� �����ϵ��� ����
        PlayerStatManager.Instance.ExplosionDamage += 20f;
    }

    void BulletExplosion(GameObject Player, Vector3 BulletPos)
    {
        GameObject explosionEffect = PhotonNetwork.Instantiate("PaidAssets/BulletExplosion", BulletPos, Quaternion.identity);

        float dmg = Player.GetComponent<PlayerState>().explosionDamage;
        float range = Player.GetComponent<PlayerState>().explosionRange;
        explosionEffect.GetComponent<BulletExplosion>().Explosion(dmg, range);
    }
}
