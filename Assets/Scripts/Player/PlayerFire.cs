using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int currentBulletCount;

    [SerializeField]
    private Image reloadTimeUI;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        state = GetComponent<PlayerState>();

        bulletCountUIs = new GameObject[state.maxBulletCount];
        if (photonView.IsMine == true)
        {
            bulletPools = new GameObject[state.maxBulletCount * 2]; // �ִ� ��ź���� 2�踦 �̸� �����.

            for (int i = 0; i < state.maxBulletCount * 2; ++i)
            {
                bulletPools[i] = PhotonNetwork.Instantiate(bulletName, firePos.position, gunPivot.transform.rotation);
                bulletPools[i].GetComponent<Bullet>().shootPlayer = this.gameObject;
            }

            // ó�� UI�� ������ X Y ��ǥ
            float xPos = -0.45f;
            float yPos = -0.3f;
            for (int i = 0; i < state.maxBulletCount; ++i)
            {
                bulletCountUIs[i] = PhotonNetwork.Instantiate("BulletCountUI", Vector3.zero, Quaternion.identity);
                bulletCountUIs[i].GetComponent<BulletCountUI>().SetParent(photonView.ViewID);
                bulletCountUIs[i].transform.localPosition = new Vector3(xPos, yPos, 0f);
                yPos += 0.15f;  // �� UI���� �Ÿ�
            }
        }

        currentBulletCount = state.maxBulletCount;
    }

    [PunRPC]
    void T(GameObject g)
    {
        g.transform.parent = this.gameObject.transform;
    }

    void Update()
    {
        if (photonView.IsMine == true)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GunPivotSetting();

            Fire();

            if(Input.GetKeyDown(KeyCode.R) && canFire == true)
            {
                photonView.RPC("Reload", RpcTarget.All);
                //Reload();
            }
        }
    }

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canFire == false)
            {
                return;
            }

            if(currentBulletCount <= 0)
            {
                Reload();
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

                photonView.RPC("DisableBulletUI", RpcTarget.All, currentBulletCount - 1);
                --currentBulletCount;
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
    void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        canFire = false;

        // ��ٸ��� (���� ��Ÿ��)
        //yield return new WaitForSeconds(state.reloadTime);
        float runTime = 0.0f;      
        while (runTime < state.reloadTime)
        {
            runTime += Time.deltaTime;

            reloadTimeUI.fillAmount = Mathf.Lerp(0, 1, runTime / state.reloadTime);

            yield return null;
        }

        reloadTimeUI.fillAmount = 0f;

        // �Ѿ� �ٽ� ä���
        currentBulletCount = state.maxBulletCount;

        if(photonView.IsMine == true)
        {
            // ��ź�� UI�ٽ� Ȱ��ȭ ��Ű��
            for (int i = 0; i < state.maxBulletCount; ++i)
            {
                bulletCountUIs[i].SetActive(true);
                photonView.RPC("EnableBulletUI", RpcTarget.All, i);
            }
        }

        canFire = true;
    }

    [PunRPC]
    void DisableBulletUI(int idx)
    {
        bulletCountUIs[idx].SetActive(false);
    }

    [PunRPC]
    void EnableBulletUI(int idx)
    {
        //if(photonView.IsMine == true)
        //{
        //    return;
        //}
        bulletCountUIs[idx].SetActive(true);
    }
}
