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
    [SerializeField]
    private Transform firePos;

    private bool canFire = true;

    private GameObject[] bulletPools;
    private GameObject selectBullet;

    [SerializeField]
    private string bulletName;
    [SerializeField]
    private string bulletCountUIName;

    private GameObject[] bulletCountUIs;
    private int remainingBullet;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        state = GetComponent<PlayerState>();

        if (photonView.IsMine == true)
        {
            bulletPools = new GameObject[state.maxBulletCount * 2]; // �ִ� ��ź���� 2�踦 �̸� �����.
            bulletCountUIs = new GameObject[state.maxBulletCount];

            for (int i = 0; i < state.maxBulletCount * 2; ++i)
            {
                bulletPools[i] = PhotonNetwork.Instantiate(bulletName, firePos.position, gunPivot.transform.rotation);
                bulletPools[i].SetActive(false);
            }

            for (int i = 0; i < state.maxBulletCount; ++i)
            {
                bulletCountUIs[i] = PhotonNetwork.Instantiate(bulletCountUIName, Vector3.zero, Quaternion.identity);
                bulletCountUIs[i].transform.SetParent(transform);
            }

            Reload();
        }
    }

    void Reload()
    {
        float xPos = -0.45f;
        float yPos = -0.3f;

        for(int i = 0; i < state.maxBulletCount; ++i)
        {
            bulletCountUIs[i].transform.localPosition = new Vector3(xPos, yPos, 0f);
            bulletCountUIs[i].SetActive(true);
            yPos += 0.15f;
        }

        remainingBullet = state.maxBulletCount;
    }

    void Update()
    {
        if (photonView.IsMine == true)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GunPivotSetting();

            Fire();
        }
    }

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canFire == false || remainingBullet <= 0)
            {
                return;
            }

            StartCoroutine(ShootingDelay());

            Vector2 toMousePosition = mousePosition - (Vector2)gunPivot.position;

            float dot = Vector2.Dot(gunPivot.right, toMousePosition);   // ��- ��+ Ȯ�� ����
            float angle = Mathf.Acos(dot / toMousePosition.magnitude) * Mathf.Rad2Deg;    // ���콺 �����Ϳ��� ����

            selectBullet = GetBullet();
            if(selectBullet != null)
            {
                selectBullet.SetActive(true);
                selectBullet.transform.position = firePos.position;
                selectBullet.GetComponent<Bullet>().Shoot(mousePosition, state.bulletPower, state.attackDamage);

                bulletCountUIs[remainingBullet - 1].SetActive(false);
                --remainingBullet;
            }
        }
    }

    void GunPivotSetting()
    {
        Vector2 toMousePosition = mousePosition - (Vector2)gunPivot.position;

        float dot = Vector2.Dot(gunPivot.right, toMousePosition);   // ��- ��+
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

    IEnumerator ShootingDelay()
    {
        canFire = false;
        yield return new WaitForSeconds(state.fireDelay);
        canFire = true;
    }

    GameObject GetBullet()
    {
        for (int i = 0; i < bulletPools.Length; ++i)
        {
            if (bulletPools[i].activeSelf == false)
            {
                return bulletPools[i];
            }
        }

        return null;
    }
}
