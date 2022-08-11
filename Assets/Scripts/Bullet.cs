using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
        float dot = Vector2.Dot(transform.right, GetComponent<Rigidbody2D>().velocity);   // 좌- 우+ 확인 가능
        float angle = Mathf.Acos(dot / GetComponent<Rigidbody2D>().velocity.magnitude) * Mathf.Rad2Deg;    // 마우스 포인터와의 각도

        if (float.IsNaN(angle) == true)
        {
            return;
        }

        if (Vector2.Dot(transform.up, GetComponent<Rigidbody2D>().velocity) >= 0.0f)    // +면 위 -면 아래에 마우스 포인터가 존재함
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, -angle);
        }
    }
}
