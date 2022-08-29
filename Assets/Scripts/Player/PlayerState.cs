using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class PlayerState : MonoBehaviour//, IPunObservable
{
    public float maxHP = 100f;
    public float HP;
    public float moveSpeed = 10f;
    public float jumpPower = 10f;

    public float fireDelay = 0.2f;
    public float bulletPower = 100f;

    public float attackDamage = 40f;

    public float explosionDamage = 20f;
    public float explosionRange = 1f;

    public int maxBulletCount = 3;

    public float reloadTime = 1.0f;

    [SerializeField]
    private Image hpBar;

    [HideInInspector]
    public PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        HP = maxHP;
    }
    // 외부에서 RPC를 호출하도록 도와주는 헬퍼함수
    public void GetDamage(float Damage)
    {
        photonView.RPC("GetDamageRPC", RpcTarget.All, Damage);
    }

    [PunRPC]
    void GetDamageRPC(float Damage)
    {
        HP -= Damage;
        HpBarUpdate();
    }

    void HpBarUpdate()
    {
        hpBar.fillAmount = HP / maxHP;
    }
}
