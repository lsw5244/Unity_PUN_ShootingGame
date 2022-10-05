using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class PlayerState : MonoBehaviour//, IPunObservable
{
    public float    maxHP;
    public float    HP;
    public float    moveSpeed;
    public float    jumpPower;

    public float    fireDelay;
    public float    bulletPower;

    public float    attackDamage;

    public float    explosionDamage;
    public float    explosionRange;

    public float    poisonDamage;
    public int      poisonCount;

    public int      maxBulletCount;

    public float    reloadTime;

    [SerializeField]
    private Image hpBar;

    [HideInInspector]
    public PhotonView photonView;

    private GameSceneManager gameSceneManager;

    [SerializeField]
    private GameObject dieEffect;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if(photonView.IsMine == true)
        {
            StatusInit();
        }
        gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
    }

    void StatusInit()
    {
        maxHP = PlayerStatusManager.Instance.MaxHP;
        HP = maxHP;
        moveSpeed = PlayerStatusManager.Instance.MoveSpeed;
        jumpPower = PlayerStatusManager.Instance.JumpPower;

        fireDelay = PlayerStatusManager.Instance.FireDelay;
        bulletPower = PlayerStatusManager.Instance.BulletPower;

        attackDamage = PlayerStatusManager.Instance.AttackDamage;

        explosionDamage = PlayerStatusManager.Instance.ExplosionDamage;
        explosionRange = PlayerStatusManager.Instance.ExplosionRange;

        poisonDamage = PlayerStatusManager.Instance.PoisonDamage;
        poisonCount = PlayerStatusManager.Instance.PoisonCount;

        maxBulletCount = PlayerStatusManager.Instance.MaxBulletCount;

        reloadTime = PlayerStatusManager.Instance.ReloadTime;

        Debug.Log($"내 플래이어 스텟 Init MaxBulletcount : {maxBulletCount}");
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

        if(PhotonNetwork.IsMasterClient == true)
        {
            if (HP <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        if (photonView.IsMine == true)
        {
            // 마스터가 죽었을 때 처리
            gameSceneManager.EndGame(PlayerType.Pink);
        }
        else
        {
            // 클라가 죽었을 때 처리
            gameSceneManager.EndGame(PlayerType.Blue);
        }

        photonView.RPC("DieRPC", RpcTarget.All);
    }

    [PunRPC]
    void DieRPC()
    {
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<PlayerFire>().enabled = false;
    }

    void HpBarUpdate()
    {
        hpBar.fillAmount = HP / maxHP;
    }
}
