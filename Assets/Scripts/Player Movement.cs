using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float playerSpeed = 5f;
    float jumpSpeed = 14f;
    float climbSpeed = 5f;
    float initialGravityScale;
    bool isAlive = true;
    Vector2 deathKick = new Vector2 (20, 20);
    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        initialGravityScale = playerRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive){return;}
        Run();
        FlipPlayer();
        ClimbPlayer();
        Die();
    }

    void OnMove(InputValue value) {
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) {
        if(!isAlive){return;}
        if(playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && value.isPressed)
        {
            playerRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
        // if(playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) && value.isPressed)
        // {
        //     playerRigidBody.velocity += new Vector2(0f, jumpSpeed);
        // }
    }

    void Run() {
        if(!isAlive){return;}
        Vector2 playerVelocity = new Vector2(moveInput.x * playerSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;
        
        bool hasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", hasHorizontalSpeed);
        
    }

    void FlipPlayer() {
        bool hasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        
        if(hasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1f);
        }
    }

    void ClimbPlayer() {
        if(!isAlive){return;}
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {
            playerRigidBody.gravityScale = initialGravityScale;
            playerAnimator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, moveInput.y * climbSpeed);
        playerRigidBody.velocity = climbVelocity;
        playerRigidBody.gravityScale = 0f;

        bool hasVerticalSpeed = Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", hasVerticalSpeed);
    }

    void Die() {
        if(playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) {
            isAlive = false;
            playerAnimator.SetTrigger("death");
            playerRigidBody.velocity = deathKick;
        }
    }
}
