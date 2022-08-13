using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float HP = 100f;
    public float moveSpeed = 10f;
    public float jumpPower = 10f;

    public float fireDelay = 0.2f;
    public float bulletPower = 100f;
    public float attackDamage = 40f;

    public int maxBulletCount = 3;
}
