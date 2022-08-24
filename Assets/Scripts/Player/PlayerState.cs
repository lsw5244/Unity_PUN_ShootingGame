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

    private PhotonView photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 마스터가 다른사람에게 데이터 보내기
            stream.SendNext(HP);
            Debug.Log($"마스터가 HP값을 보냈다 !!!, 보낸 HP값 {HP}");
        }
        else
        {
            // 클라이언트가 데이터 받기
            HP = (float)stream.ReceiveNext();
            Debug.Log($"마스터에게 Hp값을 받았다 !!!, 변화 한 수치{HP}");
        }
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    void GetDamage(float Damage)
    {
        HP -= Damage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3) && photonView.IsMine == true)
        {
            if (PhotonNetwork.IsMasterClient == true)
            {
                Debug.Log($"{gameObject.name}의 GetDamage실행");
                GetDamage(10f);
            }
            else
            {
                photonView.RPC("GetDamage", RpcTarget.MasterClient, 10f);
                Debug.Log("내건데 마스터는 아닐 때(RPC로 마스터에게 호출 요청)");
            }
        }
    }
}
