using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    public Transform groundCheck, cameraTarget;
    public LayerMask groundLayer;
    public ParticleSystem splash, trail, trail2, trailCloak;
    public float baseSpeed, jumpHeight, lowJumpMult = 0.1f, fallMult = 1.5f, maxDash = 0.4f, minDash = 0.2f;
    private float move, cameraPan, cameraOffset = -0.2f, dashCooldown = 0, speedBoost = 1f, speed;
    private bool jump, run, grounded, falling, jumpPeak, dashing = false, attack = false;
    [HideInInspector]
    public bool controller, crouch, canHit = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        splash = Instantiate(splash, transform.position, transform.rotation);
        speed = baseSpeed;
    }

    void Update()
    {
        if ((!run && Input.GetButton("Run")) || (!jump && Input.GetButton("Jump")) || (!jumpPeak && rb.velocity.y < 6 && !grounded))
        {
            anim.SetBool("transition", true);
        }

        PlayerInput();

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("AirAttack"))
        {
            attack = true;
        }

        //inverting to facing direction
        if (move < 0 && !dashing) {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        } else if (move > 0 && !dashing)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }

        //ground detection
        if (Physics2D.Linecast(transform.position, groundCheck.position, groundLayer))
        {
            grounded = true;
            falling = false;
        }
        else
        {
            grounded = false;
        }
        //peak range
        if (!grounded && rb.velocity.y < 6 & rb.velocity.y > -4){
            jumpPeak = true;
        }
        else
        {
            jumpPeak = false;
        }

        if (!grounded)
        {
            crouch = false;
            dashing = false;
        }

        if (crouch || dashing)
        {
            run = false;
            move = 0f;
        }

        if (falling || jumpPeak)
        {
            trail.Stop();
            trail2.Stop();
            trailCloak.Stop();
        }
        else if (jump)
        {
            trail.Play();
            trail2.Play();
            trailCloak.Play();
        }
        

        if (attack || anim.GetCurrentAnimatorStateInfo(0).IsName("DashEnd"))
        {
            anim.SetBool("dashing", false);
            anim.SetBool("jump", false);
            anim.SetBool("jumpPeak", false);
            anim.SetBool("falling", false);
            anim.SetBool("crouch", false);
            anim.SetBool("run", false);
            anim.SetBool("grounded", grounded);
            anim.SetFloat("move", 0f);
            anim.SetBool("dashEnd", false);
        }
        else{
            anim.SetBool("dashing", dashing);
            anim.SetBool("jump", jump);
            anim.SetBool("jumpPeak", jumpPeak);
            anim.SetBool("falling", falling);
            anim.SetBool("crouch", crouch);
            anim.SetBool("run", run);
            anim.SetBool("grounded", grounded);
            anim.SetFloat("move", Mathf.Abs(move));
        }
        anim.SetBool("attack", attack);
        MoveCamera();
    }

    private void FixedUpdate()
    {
        //faster fall
        if (rb.velocity.y < -0.1f) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMult * Time.deltaTime;
            falling = true;
        }
        //variable height
        if (rb.velocity.y > 0 && !jump) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMult * Time.deltaTime;
        }

        Dash();

        if (crouch && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
        }
        else if (run && !dashing && grounded && !crouch) //running
        {
            rb.velocity = new Vector2(move * speed * 3, rb.velocity.y);
        }
        else if (run && !dashing && !grounded) //run in air
        {
            rb.velocity = new Vector2(Mathf.Lerp(move * speed * 3, rb.velocity.x, Time.deltaTime * 5), rb.velocity.y);
        }
        else if (!dashing && !grounded) //walk in air
        {
            rb.velocity = new Vector2(Mathf.Lerp(move * speed, rb.velocity.x, Time.deltaTime * 5), rb.velocity.y);
        }
        else if (!dashing && !crouch)//walking
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
        }

        if (jump && grounded && !dashing && !attack)//jump
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    void MoveCamera()
    {
        //temp height, 0.3 = % above ground from player, 3.5 = offset from ground   
        cameraTarget.position = new Vector2(transform.position.x + transform.right.x +(move * 3)
            ,(transform.position.y * 0.3f) + cameraOffset);   
    }

    void Dash()
    {       
        if (dashCooldown > 0 && !dashing) //delay to dash again
        {
            dashCooldown -= Time.deltaTime;// * 2;  //rate of recovery
            dashing = false;
            //speed = baseSpeed * (1f + (dashCooldown*6));
        }
        else if (dashing && grounded) //dashing
        {
            dashCooldown += Time.deltaTime;
            if (dashCooldown >= maxDash || Input.GetButtonUp("Dash"))
            {
                dashing = false;
                anim.SetBool("dashEnd", true);
                if (Input.GetButtonUp("Dash"))
                {
                    dashCooldown = maxDash * 0.75f;  //if shortened dash half cooldown
                    dashing = false;
                    anim.SetBool("dashEnd", true);
                }
            }
            run = false;
            crouch = false;
            jump = false;
            move = 0f;
            //speed x direction (top speed - speed of deceleration)
            rb.velocity = new Vector2(speed * transform.right.x * (10f - (dashCooldown * 23)), rb.velocity.y);
        }
    }

    void PlayerInput()
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
            else if (dashCooldown > minDash && dashing)
            {
                dashing = Input.GetButton("Square");
            }

            jump = Input.GetButton("X");
            run = Mathf.Abs(move) > 0.85f ? true : false;
            attack = Input.GetButtonDown("Triangle");
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
            else if (dashCooldown > minDash && dashing)
            {
                dashing = Input.GetButton("Dash");
            }

            //Debug.Log(dashing + " " + anim.GetBool("dashEnd"));

            jump = Input.GetButton("Jump");
            run = Input.GetButton("Run");
            attack = Input.GetButton("Attack");
        }
    }

    void Splash(float offset)
    {
        if (move > 0)
        {
            splash.transform.position = new Vector3(transform.position.x + offset, -4.5f, transform.position.z);
        }
        else
        {
            splash.transform.position = new Vector3(transform.position.x - offset, -4.5f, transform.position.z);
        }

        if (offset > 1)
        {
            splash.Emit(20);
        }
        else
        {
            splash.Emit(7);
        }
    }

    void CanHit(float can)
    {
        if (can == 1) {
            canHit = true;
        }
        else
        {
            canHit = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 12 && canHit)
        {
            //Debug.Log("PlayerHit");
        }
    }
}
