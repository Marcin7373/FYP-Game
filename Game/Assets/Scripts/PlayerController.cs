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
    private float move, cameraOffset = 3;
    private bool jump, run, isGrounded, facingRight, isFalling;

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
        jump = Input.GetButton("Jump");
        run = Input.GetButton("Fire1");
       
        //inverting sprite for facing direction
        if (move > 0){
            facingRight = true;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
        else if (move < 0){
            facingRight = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }

        //Animations
        if (move != 0 && !run && isGrounded){
            animator.Play("Walk");
        }
        else if (run && move != 0 && isGrounded){
            animator.Play("Run");
        }
        else if (!isGrounded && !isFalling){
            animator.Play("Jump");
        }
        else if (!isGrounded && isFalling)
        {
            animator.Play("Jump");
        }
        else{
            animator.Play("Idle");
        }

        MoveCamera();
    }

    private void FixedUpdate()
    {
        //variable hight and faster fall
        if (rb.velocity.y < 0){
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMult * Time.deltaTime;
            isFalling = true;
        }
        else if (rb.velocity.y > 0 && !jump){
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMult * Time.deltaTime;
        }

        if (Physics2D.Linecast(transform.position, groundCheck.position, groundLayer)){
            isGrounded = true;
            isFalling = false;
        }
        else {
            isGrounded = false;
        }

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
}
