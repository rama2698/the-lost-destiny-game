using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float playerSpeed = 5f;
    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipPlayer();
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void Run() {
        Vector2 playerVelocity = new Vector2(moveInput.x * playerSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;
    }

    void FlipPlayer() {
        bool hasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        
        if(hasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1f);
        }
    }
}
