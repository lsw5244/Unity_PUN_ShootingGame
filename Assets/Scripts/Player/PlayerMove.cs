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

    public bool canMove = true;
    private bool isGround = false;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerState>();
        photonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if(photonView.IsMine == true && canMove == true)
        {
            Move();
        }
    }

    void Update()
    {
        if (photonView.IsMine == true && canMove == true && isGround == true)
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
            rigi.velocity = Vector2.zero;
            rigi.AddForce(Vector2.up * state.jumpPower);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
    }
}
