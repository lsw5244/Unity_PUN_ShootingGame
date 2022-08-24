using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerState : MonoBehaviour, IPunObservable
{
    public float HP = 100f;
    public float moveSpeed = 10f;
    public float jumpPower = 10f;

    public float fireDelay = 0.2f;
    public float bulletPower = 100f;
    public float attackDamage = 40f;

    public int maxBulletCount = 3;

    public float reloadTime = 1.0f;

    [HideInInspector]
    public PhotonView photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 소유자가 다른사람에게 데이터 보내기
            stream.SendNext(HP);
            Debug.Log($"{gameObject.name}에서의 Send {HP}");
        }
        else
        {
            // 클라이언트가 데이터 받기
            this.HP = (float)stream.ReceiveNext();
            Debug.Log($"{gameObject.name}에서의 Recive 받은 값 : {HP}");
        }
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void GetDamage(float Damage)
    {
        if (photonView.IsMine == true)
        {
            HP -= Damage;
        }
    }
}
