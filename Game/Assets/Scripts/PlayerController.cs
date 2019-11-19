using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public Transform groundCheck, cameraTarget;
    public LayerMask groundLayer;
    public float speed, jumpHeight, lowJumpMult = 0.1f, fallMult = 1.5f;
    private float move, cameraPan, cameraOffset = 3, dashCooldown = 0;
    private bool jump, run, isGrounded, isFalling, crouch, dashing = false;
    public bool controller = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        ControllerInput();

        //inverting sprite for facing direction
        if (move < 0 && !dashing) {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        } else if (move > 0 && !dashing)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }

        //Animations
        if (dashing && isGrounded){
            animator.Play("Dash");
        }
        else if (!isGrounded && rb.velocity.y < 6 & rb.velocity.y > -4){
            animator.Play("JumpPeak");
        }
        else if (!isGrounded && !isFalling){
            animator.Play("Jump");
        }
        else if (isFalling){
            animator.Play("Falling");
        }
        else if (crouch){
            animator.Play("Crouch");
        }
        else if (move != 0 && !run && isGrounded){
            animator.Play("Walk");
        }
        else if (run && move != 0 && isGrounded){
            animator.Play("Run");
        }                           //finding peak of = velocity.y < at peak
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

        Dash();

        if (crouch && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
        }
        else if (run && !dashing && isGrounded && !crouch) //running
        {
            rb.velocity = new Vector2(move * speed * 3, rb.velocity.y);
        }
        else if (run && !dashing && !isGrounded) //run in air
        {
            rb.velocity = new Vector2(Mathf.Lerp(move * speed * 3, rb.velocity.x, Time.deltaTime * 5), rb.velocity.y);
        }
        else if (!dashing && !isGrounded) //walk in air
        {
            rb.velocity = new Vector2(Mathf.Lerp(move * speed, rb.velocity.x, Time.deltaTime * 5), rb.velocity.y);
        }
        else if (!dashing && !crouch)//walking
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
        }

        if (jump && isGrounded && !dashing)//jump
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    void MoveCamera()
    {
        //temp height, 0.3 = % above ground from player, 4.25 = offset from ground   
        cameraTarget.position = new Vector2(transform.position.x + transform.right.x +(move * 3)
            ,(transform.position.y * 0.3f) + 3.5f);   
    }

    void Dash()
    {
        if (dashCooldown > 0 && !dashing) //delay to dash again
        {
            dashCooldown -= Time.deltaTime * 2;
            dashing = false;
        }
        else if (dashing && isGrounded) //dashing
        {
            dashCooldown += Time.deltaTime;
            if (dashCooldown >= 0.4f)
            {
                dashing = false;
            }                           //speed x direction (top speed - speed of deceleration)
            rb.velocity = new Vector2(speed * transform.right.x * (10f - (dashCooldown * 23)), rb.velocity.y);
        }
    }

    void ControllerInput()
    {
        if (controller) //Controller or keyboard input
        {
            move = Input.GetAxisRaw("Horizontal");
            //cameraPan = Input.GetAxisRaw("RightSticX");
            crouch = Input.GetButton("Circle");
            if (!dashing && dashCooldown <= 0)
            {
                dashing = Input.GetButtonDown("Square");
                dashCooldown = 0;
            }
            jump = Input.GetButton("X");
            run = Mathf.Abs(move) > 0.85f ? true : false;
        }
        else
        {
            move = Input.GetAxisRaw("Horizontal");
            crouch = Input.GetButton("Crouch");
            if (!dashing && dashCooldown <= 0)
            {
                dashing = Input.GetButtonDown("Dash");
                dashCooldown = 0;
            }
            jump = Input.GetButton("Jump");
            run = Input.GetButton("Run");
        }
    }
}
