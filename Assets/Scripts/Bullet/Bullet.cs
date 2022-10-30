using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigi;
    private PhotonView photonView;
    public GameObject shootPlayer;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();

        gameObject.SetActive(false);
    }

    public void Shoot(Vector2 mousePosition, float bulletPower, float attackDamage, Vector3 position)
    {
        //transform.position = position;
        photonView.RPC("Fire", RpcTarget.All, mousePosition, bulletPower, attackDamage, position);
    }

    [PunRPC]
    void Fire(Vector2 mousePosition, float bulletPower, float attackDamage, Vector3 position)
    {
        transform.position = position;
        LookVelocityDirection();

        gameObject.SetActive(true);

        if(GetComponent<PhotonView>().IsMine == true)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            Vector2 shootDirection = (mousePosition - (Vector2)transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(shootDirection * bulletPower);
        }
    }

    private void Update()
    {
        LookVelocityDirection();
    }

    void LookVelocityDirection()
    {
        float dot = Vector2.Dot(transform.right, rigi.velocity);   // ��- ��+
        float angle = Mathf.Acos(dot / rigi.velocity.magnitude) * Mathf.Rad2Deg;

        if (float.IsNaN(angle) == true)
        {
            return;
        }

        if (Vector2.Dot(transform.up, rigi.velocity) >= 0.0f)    // ���ϴ� ������ �� or �Ʒ����� Ȯ��
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, -angle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // ������ �� �Ѿ��� ������ �Ǵ�
            if(photonView.IsMine == true)
            {
                // �Ѿ� ����State
                float dmg = shootPlayer.GetComponent<PlayerState>().attackDamage;
                // �Ѿ˰� �浹�� �÷��̾��� State
                collision.gameObject.GetComponent<PlayerState>().GetDamage(dmg);
                // hitƯ�� �ߵ�
                if(HitAbilityManager.Instance.hitAbility != null)
                {
                    HitAbilityManager.Instance.hitAbility(shootPlayer, collision.gameObject);
                }
            }
        }
                
        if (ImpactAbilityManager.Instance.impactAbility != null && /*photonView.IsMine == true*/ shootPlayer != null)
        {
            ImpactAbilityManager.Instance.impactAbility(shootPlayer, transform.position);
        }

        this.gameObject.SetActive(false);
    }
}
