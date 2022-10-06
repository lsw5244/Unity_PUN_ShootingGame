using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
public class BulletCountUI : MonoBehaviour
{
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void SetParent(int ParentViewID)
    {
        photonView.RPC("SetParentRPC", RpcTarget.All, ParentViewID);        
    }

    [PunRPC]
    void SetParentRPC(int ParentViewID)
    {
        transform.parent = PhotonView.Find(ParentViewID).gameObject.transform;
    }

    public void SetActive(bool Active)
    {
        photonView.RPC("SetActiveRPC", RpcTarget.All, Active);
    }

    [PunRPC]
    void SetActiveRPC(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
