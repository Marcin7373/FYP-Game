using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly Dictionary<string, string[]> buttons = new Dictionary<string, string[]>()
    {{"attack", new string[] {"Attack", "Square"}}, {"jump", new string[] {"Jump", "X"}}, {"dash", new string[] {"Dash", "Circle"}}, {"fade", new string[] {"Fade", "Triangle"}}};
    private Animator anim;
    private Rigidbody2D rb;
    public Transform cameraTarget;
    public Transform[] groundChecks;
    public LayerMask groundLayer;
    public ParticleSystem splash, trail, trail2, trailCloak;
    public AudioSource[] splashSfx, attackSfx;
    public float baseSpeed, jumpHeight, lowJumpMult = 0.1f, fallMult = 1.5f, maxDash = 0.4f, minDash = 0.2f, damage = 0.05f;
    private float move, dashCooldown = 0, speed;
    private bool jump, run, grounded, falling, jumpPeak, dashing = false, attack = false, fade = false, col = false;
    private int sfxIterS = 0, cont = 0; //controller 1=yes 0=no
    [HideInInspector]
    public bool crouch;

    void Awake()
    {
        AudioListener.volume = 0f;
        for (int i = 0; i<Input.GetJoystickNames().Length;i++)
        {
            if (Input.GetJoystickNames()[i].Length > 0)
            {
                cont = Input.GetJoystickNames()[i].Length > 0 ? 1 : 0;
            }
        }
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        splash = Instantiate(splash, transform.position, transform.rotation);
        speed = baseSpeed;
    }

    void Update()
    {   //quit game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        //pause game
        if (Input.GetKeyDown("p") || Input.GetButtonDown("Options"))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
        //dying
        if (Health.Instance.CurHealth >= 2f && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            anim.SetTrigger("death");
            move = 0f;
            attack = false;
            jump = false;
            dashing = false;
            crouch = false;
            fade = false;
            falling = false;
            grounded = true;
        }
        else if (Time.timeScale == 1 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            PlayerInput();
            UpdateFlags();
        }
         
        if (anim.speed == 0f && transform.position.x > -19)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(-19.5f, -2.93f, 0), 0.09f);

        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && transform.position.x <= -19)
        {
            Health.Instance.CurHealth = 1;
            anim.speed = 1f;
        }
       
        //air rotation       
        if (!grounded && !falling)//bug at peak if direction inverting not restricted to ground
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, -(Mathf.Abs(move) * 15));
        }
        else if(falling)
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, Mathf.Abs(move) * 10);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0f);
        }
        
        //inverting to facing direction
        if ((move < 0 && !dashing && grounded) || (attack && move < 0)) {
            transform.rotation = Quaternion.Euler(0, 180, transform.eulerAngles.z);
        } else if ((move > 0 && !dashing && grounded) || (attack && move > 0))
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);
        }

        //Particle system jump trails
        if ( grounded || attack)
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
        //fade ability damage to player
        if (fade && transform.position.x > -1f)
        {
            Health.Instance.CurHealth += 0.8f * Time.deltaTime;
        }
        //Animtion flags
        anim.SetBool("dashing", dashing);
        anim.SetBool("jump", jump);
        anim.SetBool("jumpPeak", jumpPeak);
        anim.SetBool("falling", falling);
        anim.SetBool("crouch", crouch);
        anim.SetBool("run", run);
        anim.SetBool("grounded", grounded);
        if (attack)
        {
            anim.SetFloat("move", 0);
        }
        else
        {
            anim.SetFloat("move", Mathf.Abs(move));
        }
        
        anim.SetBool("fade", fade);
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

        if (rb.velocity.y < -0.1f && attack)//cancel faster fall and gravity(3) mostly while attacking
        {
            rb.velocity -= Vector2.up * Physics2D.gravity.y * (fallMult+2f) * Time.deltaTime;
        }

        //variable height
        if ((rb.velocity.y > 0 && !jump) || attack) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMult * Time.deltaTime;
        }

        Dash();
        if (!col)
        {
            if ((crouch && grounded) || attack)
            {
                rb.velocity = new Vector2(rb.velocity.x * 0.92f, rb.velocity.y);//crouch slide
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
        else
        {
            col = false;
        }
    }

    private void PlayerInput()
    {
        if (Input.GetKeyDown("q"))
        {
            AudioListener.volume -= 0.1f;
        }
        else if (Input.GetKeyDown("w"))
        {
            AudioListener.volume += 0.1f;
        }

        if (!jumpPeak && rb.velocity.y < 6 && !grounded) //transition from jump to peak
        {
            anim.SetBool("transition", true);
        }

        if (Input.GetButtonUp(buttons["dash"][cont]) && dashing && dashCooldown < maxDash)
        {
            dashCooldown = maxDash * 0.65f;  //if shortened dash half cooldown
            dashing = false;
            anim.SetBool("dashEnd", true);
        }

        move = Input.GetAxisRaw("Horizontal");

        if (cont == 1) //Controller or keyboard input
        {
            if ((!run && Mathf.Abs(move) > 0.75f ? true : false) || (!jump && Input.GetButton(buttons["jump"][cont])))
            {
                anim.SetBool("transition", true);
            }
            crouch = Input.GetAxisRaw("Vertical") < -0.4f ? true : false;
            run = Mathf.Abs(move) > 0.75f ? true : false;
        }
        else
        {
            if ((!run && Input.GetButton("Run")) || (!jump && Input.GetButton(buttons["jump"][cont])))
            {
                anim.SetBool("transition", true);
            }

            crouch = Input.GetButton("Crouch");
            run = Mathf.Abs(move) > 0.75f ? true : false;
        }

        if (!dashing && dashCooldown <= 0)
        {
            dashing = Input.GetButtonDown(buttons["dash"][cont]);
            dashCooldown = 0;
            speed = baseSpeed;
        }
        else if (dashCooldown > minDash && dashing)
        {
            dashing = Input.GetButton(buttons["dash"][cont]);
        }

        jump = Input.GetButton(buttons["jump"][cont]);
        attack = Input.GetButton(buttons["attack"][cont]);
        fade = Input.GetButton(buttons["fade"][cont]);
    }

    private void UpdateFlags()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("AirAttack"))
        {
            attack = true;
            anim.SetBool("attackTrans", false);
        }

        //ground detection
        if (Physics2D.Linecast(transform.position, groundChecks[0].position, groundLayer) || Physics2D.Linecast(transform.position, groundChecks[1].position, groundLayer) || Physics2D.Linecast(transform.position, groundChecks[2].position, groundLayer))
        {
            if (!grounded)
            {              
                Splash(0);
                sfxIterS = (sfxIterS + 1) % splashSfx.Length;
                splashSfx[sfxIterS].Play();                
            }
            grounded = true;
            falling = false;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("AirAttack"))
            {
                anim.SetBool("attackTrans", true);
            }
        }
        else
        {
            if (grounded && !splashSfx[sfxIterS].isPlaying)
            {
                splashSfx[sfxIterS].Play();
                Splash(0);
            }
            grounded = false;
        }

        //peak range
        if (!grounded && rb.velocity.y < 6 & rb.velocity.y > -4)
        {
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

        if (dashing && grounded)
        {
            run = false;
            crouch = false;
            jump = false;
            move = 0f;
            fade = false;
            attack = false;
        }

        if (attack || anim.GetCurrentAnimatorStateInfo(0).IsName("DashEnd"))
        {
            dashing = false;
            jump = false;
            jumpPeak = false;
            falling = false;
            crouch = false;
            fade = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("DashEnd"))
        {
            attack = false;
        }

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DashEnd") && !dashing)
        {
            gameObject.layer = 9;
        }
    }

    void MoveCamera()
    {
        //temp height, 0.3 = % above ground from player, 3.5 = offset from ground   
        cameraTarget.position = new Vector3(transform.position.x + transform.right.x +(move * 3)
            ,(transform.position.y * 0.22f), 0);   
    }

    void Dash()
    {
        if (dashCooldown > 0 && !dashing) //delay to dash again
        {
            dashCooldown -= Time.deltaTime / 1.5f;  //rate of recovery
            dashing = false;
            speed = baseSpeed * (1f + (dashCooldown * 3));
        }
        else if (dashing && grounded) //dashing
        {
            dashCooldown += Time.deltaTime;
            if (dashCooldown >= maxDash)
            {
                dashing = false;
                anim.SetBool("dashEnd", true);
            }      
            //speed x direction (top speed - speed of deceleration)
            rb.velocity = new Vector2(speed * transform.right.x * (13f - (dashCooldown * 23)), rb.velocity.y);
        }
    }

    void Splash(float offset)
    {
        if (transform.position.y < -3.06f)
        {
            if (move > 0)
            {
                splash.transform.position = new Vector3(transform.position.x + offset, -4.7f, transform.position.z);
            }
            else
            {
                splash.transform.position = new Vector3(transform.position.x - offset, -4.7f, transform.position.z);
            }

            if (Mathf.Abs(rb.velocity.x) > 0.9f)
            {
                splash.Emit(10);
            }
            else
            {
                splash.Emit(25);
            }
            sfxIterS = (sfxIterS + 1) % splashSfx.Length;
            splashSfx[sfxIterS].Play();
        }            
    }

    void AttackSFX(float from)
    {
        if (from > 0 && !attackSfx[0].isPlaying && !attackSfx[1].isPlaying)
        {
            attackSfx[0].time = from;
            attackSfx[0].Play();
        }
        else if (attackSfx[0].isPlaying && from == 0)
        {
            attackSfx[1].time = from;
            attackSfx[1].Play();
        }
        else if (from == 0)
        {
            attackSfx[0].time = from;
            attackSfx[0].Play();
        }       
    }

    void CanHit(float can)
    {
        if (can == 1) {
            gameObject.layer = 9;
        }
        else
        {
            gameObject.layer = 10;
        }
    }

    void SetDashEnd() => anim.SetBool("dashEnd", false);

    public void Attack()
    {
        Health.Instance.CurHealth -= damage;
    }

    void Respawn()
    {       
        anim.speed = 0f;       
    }

    //keep out of boss
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 11)
        {
            rb.velocity = new Vector2(((-(other.GetContact(0).point - (Vector2)transform.position).normalized) * 3).x, rb.velocity.y-1.5f);
            col = true;
        }
    }

    //push
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            rb.velocity = new Vector2(((other.gameObject.GetComponent<Transform>().position - transform.position).normalized * 50).x, rb.velocity.y);
            col = true;
        }
    }
}
