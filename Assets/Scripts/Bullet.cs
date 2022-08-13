using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigi;

    private void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 mousePosition, float bulletPower, float attackDamage)
    {
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
        float angle = Mathf.Acos(dot / rigi.velocity.magnitude) * Mathf.Rad2Deg;    // 마우스 포인터와의 각도

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
}
