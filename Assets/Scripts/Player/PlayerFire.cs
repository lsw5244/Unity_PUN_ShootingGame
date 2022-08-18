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
    private GameObject bulletCountUI;

    private GameObject[] bulletCountUIs;
    private int remainingBullet;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        state = GetComponent<PlayerState>();

        if (photonView.IsMine == true)
        {
            bulletPools = new GameObject[state.maxBulletCount * 2]; // �ִ� ��ź���� 2�踦 �̸� �����.

            for (int i = 0; i < state.maxBulletCount * 2; ++i)
            {
                bulletPools[i] = PhotonNetwork.Instantiate(bulletName, firePos.position, gunPivot.transform.rotation);
            }


        }

        bulletCountUIs = new GameObject[state.maxBulletCount];
        // ó�� UI�� ������ X Y ��ǥ
        float xPos = -0.45f;
        float yPos = -0.3f;
        for (int i = 0; i < state.maxBulletCount; ++i)
        {
            bulletCountUIs[i] = Instantiate(bulletCountUI);
            bulletCountUIs[i].transform.parent = this.transform;
            bulletCountUIs[i].transform.localPosition = new Vector3(xPos, yPos, 0f);
            yPos += 0.15f;  // �� UI���� �Ÿ�
        }

        remainingBullet = state.maxBulletCount;
    }

    IEnumerator ReloadCoroutine()
    {
        // ��ٸ��� (���� ��Ÿ��)
        canFire = false;

        yield return new WaitForSeconds(state.reloadTime);

        // �Ѿ� �ٽ� ä���
        remainingBullet = state.maxBulletCount;

        // ��ź�� UI�ٽ� Ȱ��ȭ ��Ű��
        for (int i = 0; i < state.maxBulletCount; ++i)
        {
            bulletCountUIs[i].SetActive(true);
            photonView.RPC("EnableBulletUI", RpcTarget.All, i);
        }

        canFire = true;
    }

    [PunRPC]
    void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    void Update()
    {
        if (photonView.IsMine == true)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GunPivotSetting();

            Fire();

            if(Input.GetKeyDown(KeyCode.R))
            {
                photonView.RPC("Reload", RpcTarget.All);
                //Reload();
                Debug.Log("Reload!!!");
            }
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
                selectBullet.transform.position = firePos.position;
                selectBullet.GetComponent<Bullet>().Shoot(mousePosition, state.bulletPower, state.attackDamage);

                photonView.RPC("DisableBulletUI", RpcTarget.All, remainingBullet - 1);
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

    [PunRPC]
    void DisableBulletUI(int idx)
    {
        bulletCountUIs[idx].SetActive(false);
    }

    [PunRPC]
    void EnableBulletUI(int idx)
    {
        bulletCountUIs[idx].SetActive(true);
    }
}
