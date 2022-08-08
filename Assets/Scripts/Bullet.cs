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
}
