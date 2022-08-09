using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerFire : MonoBehaviour
{
    private PhotonView photonView;

    private PlayerState state;

    private Vector2 mousePosition;

    [SerializeField]
    private Transform gunPivot;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        state = GetComponent<PlayerState>();
    }

    void Update()
    {
        if(photonView.IsMine == true)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GunPivotSetting();

            Fire();
        }
    }

    void Fire()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject bullet = PhotonNetwork.Instantiate("TempBullet", Vector2.zero, Quaternion.identity);
            bullet.GetComponent<Bullet>().Shoot(mousePosition, state.bulletPower, state.attackDamage);
        }
    }

    void GunPivotSetting()
    {
        Vector2 toMousePosition = mousePosition - (Vector2)gunPivot.position;

        float dot = Vector2.Dot(gunPivot.right, toMousePosition);   // ��- ��+ Ȯ�� ����
        float angle = Mathf.Acos(dot / toMousePosition.magnitude) * Mathf.Rad2Deg;    // ���콺 �����Ϳ��� ����

        if (float.IsNaN(angle) == true)
        {
            return;
        }

        if (Vector2.Dot(gunPivot.up, toMousePosition) >= 0.0f)    // +�� �� -�� �Ʒ��� ���콺 �����Ͱ� ������
        {
            gunPivot.rotation = gunPivot.rotation * Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            gunPivot.rotation = gunPivot.rotation * Quaternion.Euler(0f, 0f, -angle);
        }
    }
}
