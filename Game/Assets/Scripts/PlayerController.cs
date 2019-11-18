using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public Transform groundCheck, cameraTarget;
    public LayerMask groundLayer;
    public float speed, jumpHeight;
    private float move, cameraOffset = 3, dashCooldown = 0;
    private bool jump, run, isGrounded, facingRight, isFalling, crouch, dash, busy = false;
    public float savedVelocity;

    public float fallMult = 1.5f;
    public float lowJumpMult = 0.1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Input
        move = Input.GetAxisRaw("Horizontal");
        crouch = Input.GetKey("n");
        if (!busy)
        {
            dash = Input.GetKeyDown("b");
        }
        jump = Input.GetButton("Jump");
        run = Input.GetButton("Fire1");

        //inverting sprite for facing direction
        if (move < 0){
            facingRight = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
        else if (move > 0){
            facingRight = true;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }

        //Animations
        if (busy)
        {
            animator.Play("Dash");
        }
        if (move != 0 && !run && isGrounded){
            animator.Play("Walk");
        }
        else if (run && move != 0 && isGrounded){
            animator.Play("Run");
        }
        else if (!isGrounded && rb.velocity.y < 6 & rb.velocity.y > -4)
        {
            animator.Play("JumpPeak");
        }
        else if (!isGrounded && !isFalling){
            animator.Play("Jump");
        }
        else if (isFalling)
        {
            animator.Play("Falling");
        }
        else if (crouch)
        {
            animator.Play("Crouch");
        }
        else{
            animator.Play("Idle");
        }

        MoveCamera();
    }

    private void FixedUpdate()
    {
        //variable hight and faster fall
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMult * Time.deltaTime;
            isFalling = true;
        }
        else if (rb.velocity.y > 0 && !jump) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMult * Time.deltaTime;
        }

        if (Physics2D.Linecast(transform.position, groundCheck.position, groundLayer)) {
            isGrounded = true;
            isFalling = false;
        }
        else {
            isGrounded = false;
        }

        /*if (dash)
        {
            savedVelocity = rb.velocity.x;
            rb.velocity = new Vector2(move * speed * 5, rb.velocity.y);
            busy = true;
        }
        else if (busy)
        {
            dash = false;
            dashCooldown += Time.deltaTime * 3;
            if (dashCooldown >= 20f)
            {
                dashCooldown = 20f;
                rb.velocity = new Vector2(savedVelocity, rb.velocity.y);
                busy = false;
                dashCooldown = 0;
            }
        }
        Debug.Log(dashCooldown);
        Debug.Log(dash);
        Debug.Log(busy);
        */
        if (run) //running
        {
            rb.velocity = new Vector2(move * speed * 3, rb.velocity.y);
        }
        else //walking
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
        }

        if (jump && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    void MoveCamera()
    {
        //temp height, 0.3 = % above ground from player, 4.25 = offset from ground
        float cameraHeight = (transform.position.y * 0.3f) + 3.5f;
        if (facingRight)
        {
            cameraTarget.position = new Vector2(transform.position.x + 1 +(move * cameraOffset), cameraHeight);
        }
        else
        {
            cameraTarget.position = new Vector2(transform.position.x - 1 +(move * cameraOffset), cameraHeight);
        }
    }

    void Dash()
    {
        
        //rb.velocity = new Vector2(move * speed * 5, rb.velocity.y);
    }

    
}
