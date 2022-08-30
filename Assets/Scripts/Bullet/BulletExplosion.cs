using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BulletExplosion : MonoBehaviour
{
    [SerializeField]
    private float explosionTime = 1.0f;

    public void Explosion(float explosionDamage, float explosionRange)
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, explosionRange, LayerMask.GetMask("Player"));
        for (int i = 0; i < colls.Length; ++i)
        {
            colls[i].GetComponent<PlayerState>().GetDamage(explosionDamage);
        }

        StartCoroutine(ExplosionCoroutine());
    }

    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(explosionTime);

        Destroy(this.gameObject);
    }
}
