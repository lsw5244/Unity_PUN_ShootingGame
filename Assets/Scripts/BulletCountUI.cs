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
        transform.parent = PhotonView.Find(ParentViewID).gameObject.GetComponent<Transform>();
    }
}
