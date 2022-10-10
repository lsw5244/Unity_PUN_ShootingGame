using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private WallType wallType;

    [SerializeField]
    private float pushPower = 1000f;
    [SerializeField]
    private float freezeTime = 1f;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag("Player") == true)
        {
            coll.gameObject.GetComponent<PlayerDebuff>().StartMoveFreeze(freezeTime);
            coll.gameObject.GetComponent<PlayerState>().WallHit(wallType, pushPower);
            
        }
    }
}
