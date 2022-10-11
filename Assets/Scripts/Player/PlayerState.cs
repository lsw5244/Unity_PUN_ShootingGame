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

    public float    freezeTime;

    public int      maxBulletCount;

    public float    reloadTime;

    [SerializeField]
    private Image hpBar;

    [HideInInspector]
    public PhotonView photonView;

    private GameSceneManager gameSceneManager;

    [SerializeField]
    private GameObject dieEffect;

    private bool isDie = false;

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
        photonView.RPC("ShareMaxHP", RpcTarget.Others, maxHP);

        moveSpeed = PlayerStatusManager.Instance.MoveSpeed;
        jumpPower = PlayerStatusManager.Instance.JumpPower;

        fireDelay = PlayerStatusManager.Instance.FireDelay;
        bulletPower = PlayerStatusManager.Instance.BulletPower;

        attackDamage = PlayerStatusManager.Instance.AttackDamage;

        explosionDamage = PlayerStatusManager.Instance.ExplosionDamage;
        explosionRange = PlayerStatusManager.Instance.ExplosionRange;

        poisonDamage = PlayerStatusManager.Instance.PoisonDamage;
        poisonCount = PlayerStatusManager.Instance.PoisonCount;

        freezeTime = PlayerStatusManager.Instance.FreezeTime;

        maxBulletCount = PlayerStatusManager.Instance.MaxBulletCount;

        reloadTime = PlayerStatusManager.Instance.ReloadTime;
    }

    [PunRPC]
    void ShareMaxHP(float maxHP)
    {
        this.maxHP = maxHP;
        this.HP = this.maxHP;
    }

    // 외부에서 RPC를 호출하도록 도와주는 헬퍼함수
    // 총알을 쏜 객체(공격 한 클라이언트)가 호출한다.
    public void GetDamage(float Damage)
    {
        photonView.RPC("GetDamageRPC", RpcTarget.All, Damage);
    }

    [PunRPC]
    void GetDamageRPC(float Damage)
    {
        if(isDie == true)
        {
            return;
        }

        HP -= Damage;
        HpBarUpdate();

        if(PhotonNetwork.IsMasterClient == true)
        {
            if (HP <= 0)
            {
                isDie = true;
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
        GetComponent<PlayerFire>().DisableBulletCountUI();
        GetComponent<PlayerFire>().enabled = false;
        GetComponent<PlayerMove>().enabled = false;
    }

    void HpBarUpdate()
    {
        //hpBar.fillAmount = HP / maxHP;
        if(photonView.IsMine == true)
        {
            photonView.RPC("HpBarUpdateRPC", RpcTarget.All, HP / maxHP);
        }
    }

    [PunRPC]
    void HpBarUpdateRPC(float fillAmount)
    {
        hpBar.fillAmount = fillAmount;
    }

    public void WallHit(WallType hitWallType, float pushPower)
    {
        if(photonView.IsMine == true)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            switch (hitWallType)
            {
                case WallType.Left:
                    GetComponent<Rigidbody2D>().AddForce(Vector2.right * pushPower);
                    break;
                case WallType.Right:
                    GetComponent<Rigidbody2D>().AddForce(Vector2.left * pushPower);
                    break;
                case WallType.Top:
                    GetComponent<Rigidbody2D>().AddForce(Vector2.down * pushPower);
                    break;
                case WallType.Bottom:
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * pushPower);
                    break;
            }
        }
    }
}
