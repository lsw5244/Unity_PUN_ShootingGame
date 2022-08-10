using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigi;
    private PlayerState state;

    private Vector2 moveVelocity;
    private float moveVelocityX;

    private float horizontalInput;

    private PhotonView photonView;
    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerState>();
        photonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if(photonView.IsMine == true)
        {
            Move();
        }
    }

    void Update()
    {
        if (photonView.IsMine == true)
        {
            Jump();
        }
    }

    void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.1f)
        {
            moveVelocity.x = state.moveSpeed;
        }
        else if (horizontalInput < -0.1f)
        {
            moveVelocity.x = -state.moveSpeed;
        }
        else
        {
            moveVelocity.x = 0f;
        }

        moveVelocity.y = rigi.velocity.y;

        rigi.velocity = moveVelocity;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump!!!");
            rigi.velocity = Vector2.zero;
            rigi.AddForce(Vector2.up * state.jumpPower);
        }
    }
}