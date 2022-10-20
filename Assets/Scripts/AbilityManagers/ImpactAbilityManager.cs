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
            Debug.LogWarning($"ImpactAbility 여러개가 감지되어 {gameObject.name}를 삭제합니다.");
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
        // 폭발 데미지는 20씩 증가하도록 구현
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
