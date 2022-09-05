using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class RoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            if(Input.GetKeyDown(KeyCode.Keypad9))
            {
                PhotonNetwork.LoadLevel(2);
            }
        }
    }
}
