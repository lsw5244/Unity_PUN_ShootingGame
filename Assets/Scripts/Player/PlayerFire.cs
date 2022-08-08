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
        //if(photonView.IsMine == true)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(mousePosition);
            GunPivotSetting();

            Fire();
        }
    }

    void Fire()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //mousePosition = Input.mousePosition;
            GameObject bullet = PhotonNetwork.Instantiate("TempBullet", Vector2.zero, Quaternion.identity);
            bullet.GetComponent<Bullet>().Shoot(mousePosition, state.bulletPower, state.attackDamage);
        }
    }

    void GunPivotSetting()
    {
        Vector2 v = mousePosition - (Vector2)gunPivot.position;

        //float dot = Vector3.Dot(forward, toTarget); // +면 앞, -면 뒤에있는 것


        //float dot = Vector3.Dot(forward, toTarget); // +면 앞, -면 뒤에있는 것
        //float angle = Mathf.Acos(dot / toTarget.magnitude); // 타겟의 각도 구하기, 라디안

        // 왼쪽에 있으면 -, 오른쪽에 있으면 +  (좌 우 판별)
        //Debug.Log(Vector3.Cross(gunPivot.transform.right, v).y);
        //float crossY = Vector3.Cross(gunPivot.transform.right, v).x;  // 상, 하 판별
        //Debug.Log(crossY);


        float dot2 = Vector2.Dot(gunPivot.up, v);   // +면 위 -면 아래
                                                    //Debug.Log(dot2);

        float dot = Vector2.Dot(gunPivot.right, v);     // 상+ 하- 판별
        float angle = Mathf.Acos(dot / v.magnitude) * Mathf.Rad2Deg;
        Debug.Log($"dot :{dot}  angle : {angle/* * Mathf.Rad2Deg*/}");

        if (float.IsNaN(angle) == true)
        {
            return;
        }

        if (Vector2.Dot(gunPivot.up, v) >= 0.0f)
        {
            gunPivot.rotation = gunPivot.rotation * Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            gunPivot.rotation = gunPivot.rotation * Quaternion.Euler(0f, 0f, -angle);
        }




    }
}
