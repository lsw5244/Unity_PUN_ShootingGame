using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BulletExplosion : MonoBehaviour
{
    [SerializeField]
    private float explosionTime = 1.0f;

    public float explosionRange = 5.0f;
    public float explosionDamage;
    

    void Explosion()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {

        }
    }

    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(explosionTime);

        Destroy(this.gameObject);
    }
}
