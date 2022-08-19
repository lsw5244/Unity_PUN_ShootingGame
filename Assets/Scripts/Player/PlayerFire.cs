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
    private int remainingBullet;

    [SerializeField]
    private Image reloadTimeUI;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        state = GetComponent<PlayerState>();

        if (photonView.IsMine == true)
        {
            bulletPools = new GameObject[state.maxBulletCount * 2]; // 최대 장탄수의 2배를 미리 만든다.

            for (int i = 0; i < state.maxBulletCount * 2; ++i)
            {
                bulletPools[i] = PhotonNetwork.Instantiate(bulletName, firePos.position, gunPivot.transform.rotation);
            }
        }

        bulletCountUIs = new GameObject[state.maxBulletCount];
        // 처음 UI가 놓여질 X Y 좌표
        float xPos = -0.45f;
        float yPos = -0.3f;
        for (int i = 0; i < state.maxBulletCount; ++i)
        {
            bulletCountUIs[i] = Instantiate(bulletCountUI);
            bulletCountUIs[i].transform.parent = this.transform;
            bulletCountUIs[i].transform.localPosition = new Vector3(xPos, yPos, 0f);
            yPos += 0.15f;  // 각 UI간의 거리
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

            if(remainingBullet <= 0)
            {
                Reload();
                return;
            }

            StartCoroutine(ShootingDelay());

            Vector2 toMousePosition = mousePosition - (Vector2)gunPivot.position;

            float dot = Vector2.Dot(gunPivot.right, toMousePosition);   // 좌- 우+ 확인 가능
            float angle = Mathf.Acos(dot / toMousePosition.magnitude) * Mathf.Rad2Deg;    // 마우스 포인터와의 각도

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

        float dot = Vector2.Dot(gunPivot.right, toMousePosition);   // 좌- 우+
        float angle = Mathf.Acos(dot / toMousePosition.magnitude) * Mathf.Rad2Deg;    // 마우스 포인터와의 각도

        if (float.IsNaN(angle) == true)
        {
            return;
        }

        if (Vector2.Dot(gunPivot.up, toMousePosition) >= 0.0f)    // +면 위 -면 아래에 마우스 포인터가 존재함
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

        // 기다리기 (장전 쿨타임)
        //yield return new WaitForSeconds(state.reloadTime);
        float runTime = 0.0f;      
        while (runTime < state.reloadTime)
        {
            runTime += Time.deltaTime;

            reloadTimeUI.fillAmount = Mathf.Lerp(0, 1, runTime / state.reloadTime);

            yield return null;
        }

        reloadTimeUI.fillAmount = 0f;


        // 총알 다시 채우기
        remainingBullet = state.maxBulletCount;

        // 장탄수 UI다시 활성화 시키기
        for (int i = 0; i < state.maxBulletCount; ++i)
        {
            bulletCountUIs[i].SetActive(true);
            photonView.RPC("EnableBulletUI", RpcTarget.All, i);
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
        bulletCountUIs[idx].SetActive(true);
    }
}
