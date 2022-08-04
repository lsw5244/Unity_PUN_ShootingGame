using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigi;
    private PlayerState state;

    private Vector2 moveVelocity;
    private float moveVelocityX;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerState>();
    }

    void Update()
    {
        //rigi.velocity = new Vector2(state.moveSpeed, rigi.velocity.y);
    }

    private void FixedUpdate()
    {
        //rigi.velocity = new Vector2(state.moveSpeed, rigi.velocity.y);

        float h = Input.GetAxis("Horizontal");
        Debug.Log(h);

        if(h > 0.1f)
        {
            moveVelocity.x = state.moveSpeed;
        }
        else if(h < -0.1f)
        {
            moveVelocity.x = -state.moveSpeed;
        }
        else
        {
            moveVelocity.x = 0f;
        }

        rigi.velocity = moveVelocity;

    }
}
