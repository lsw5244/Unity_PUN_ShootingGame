using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BulletCountUI : MonoBehaviour
{
    private Vector3 localPosition;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void Init(Vector3 pos, Transform parent, bool ActiveValue)
    {
        SetLocalPosition(pos);
        //SetParentTransform(parent);
        ActiveSetting(ActiveValue);
    }

    public void SetLocalPosition(Vector3 pos)
    {
        localPosition = pos;
        photonView.RPC("PositionSetting", RpcTarget.All);
    }

    [PunRPC]
    void PositionSetting()
    {
        transform.localPosition = localPosition;
    }

    public void ActiveSetting(bool value)
    {
        photonView.RPC("ChangeActiveState", RpcTarget.All, value);
    }

    [PunRPC]
    void ChangeActiveState(bool value)
    {
        gameObject.SetActive(value);
    }

    public void SetParentTransform(Transform parent)
    {
        photonView.RPC("ChangeParentTransform", RpcTarget.All, parent);
    }

    [PunRPC]
    void ChangeParentTransform(Transform parent)
    {
        transform.parent = parent;
    }


}
