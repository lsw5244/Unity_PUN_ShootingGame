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

    public void Shoot(Vector2 mousePosition, float bulletPower, float attackDamage)
    {
        photonView.RPC("Fire", RpcTarget.All, mousePosition, bulletPower, attackDamage);
    }

    [PunRPC]
    void Fire(Vector2 mousePosition, float bulletPower, float attackDamage)
    {
        gameObject.SetActive(true);
        Vector2 shootDirection = (mousePosition - (Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(shootDirection * bulletPower);
    }

    private void Update()
    {
        LookVelocityDirection();
    }

    void LookVelocityDirection()
    {
        float dot = Vector2.Dot(transform.right, rigi.velocity);   // 좌- 우+
        float angle = Mathf.Acos(dot / rigi.velocity.magnitude) * Mathf.Rad2Deg;

        if (float.IsNaN(angle) == true)
        {
            return;
        }

        if (Vector2.Dot(transform.up, rigi.velocity) >= 0.0f)    // 향하는 방향이 위 or 아래인지 확인
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
        

        if (ImpactAbilityManager.Instance.impactAbility != null && shootPlayer != null)
        {
            ImpactAbilityManager.Instance.impactAbility(shootPlayer, transform.position);
        }
        
        this.gameObject.SetActive(false);
    }
}
