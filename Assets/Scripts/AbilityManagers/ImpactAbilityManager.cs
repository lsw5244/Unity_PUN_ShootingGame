using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class ImpactAbilityManager : MonoBehaviour
{
    public static ImpactAbilityManager Instance;

    public delegate void ImpactAbilityDelegate(GameObject Player, Vector3 pos);
    public ImpactAbilityDelegate impactAbility;

    private PhotonView photonView;

    [SerializeField]
    private GameObject explosionEmpact;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning($"ImpactAbility �������� �����Ǿ� {gameObject.name}�� �����մϴ�.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            impactAbility += BulletExplosion;
            Debug.Log("BulletExplosion �߰�");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            impactAbility -= BulletExplosion;
            Debug.Log("BulletExplosion ����");
        }
    }

    void BulletExplosion(GameObject Player, Vector3 pos)
    {
        photonView.RPC("RPCBulletExplosion", RpcTarget.All, Player, pos);
    }

    [PunRPC]
    void RPCBulletExplosion(GameObject Player, Vector3 pos)
    {
        Instantiate(explosionEmpact, pos, Quaternion.identity);
    }

}
