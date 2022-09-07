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

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                photonView.RPC("SetActiveRPC", RpcTarget.All, true);
            }

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                photonView.RPC("SetActiveRPC", RpcTarget.All, false);
            }
        }
        
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

    public void SetActive(bool Active)
    {
        photonView.RPC("SetActiveRPC", RpcTarget.All, Active);
    }

    [PunRPC]
    void SetActiveRPC(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void AAA(bool a)
    {
        Debug.Log("AAA가 호출되었다 !!!");
    }
}
