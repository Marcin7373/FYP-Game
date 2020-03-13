using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI.Components;
using Apex.AI;
using System;

public class Brain : MonoBehaviour, IContextProvider
{                                        //Bite Laser Swipe  TailS
    private float[] damage = new float[] { 0.2f, 0.1f, 0.1f, 0.1f };
    private List<RaycastHit2D> rays = new List<RaycastHit2D>();
    public Transform eyes, longTail, shortTail, healthScale;
    public ParticleSystem laser;
    private bool faceLeft = true;
    public float speed = 1f, playerDamage = 0.1f, health, dmgScale = 1f;
    private Rigidbody2D rb;
    private Animator anim;
    public Hashtable playerInfo = new Hashtable();
    private AIContext context;           //Current   Bite      Laser     Swipe    TailSwipe
    public int[,] history = new int[,] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };

    private void Awake()
    {
        playerInfo["timePassed"] = 10f;
        playerInfo["position"] = new Vector3(-27, -4, 0);
        playerInfo["velocity"] = new Vector2(0,0);
        playerInfo["crouch"] = false;
        health = healthScale.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        context = new AIContext(this.transform, eyes, rb, anim, speed, false, playerInfo, history);
    }

    void Update()
    {
        HealthUpdate();
        
        /*rays = eyes.rays;
        playerInfo = eyes.playerInfo;
        if (rays.Count == 5)
        {
            SensorResponse();
        }*/
    }

    private void SensorResponse()
    {        
        if ((bool)playerInfo["isBehind"] && faceLeft)
        { 
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            //eyes.behind = false;
            faceLeft = false;
        }
        else if ((bool)playerInfo["isBehind"] && !faceLeft)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            //eyes.behind = false;
            faceLeft = true;
        }

        if (rays[0] && rays[1] && rays[2] && faceLeft)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }else if (rays[0] && rays[1] && rays[2] && !faceLeft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }

    void Laser(float on)
    {
        if (on == 1){
            laser.Play();
        }
        else{
            laser.Stop();
        }
    }

    void TailSwipe()
    {
        longTail.transform.position = new Vector2(((Vector3)playerInfo["position"]).x, longTail.transform.position.y);
    }

    void NotBusy()
    {
        context.busy = false;
    }

    public void Attacks(bool col)
    {
        if (col && context.history[0,0] == 2)
        {
            health+=(damage[context.history[0, 0]-1] * Time.deltaTime) * dmgScale;
        }
        else if (!col)
        {
            context.history[(context.history[0, 0]), 1]++;  // times attack hit
            health += damage[context.history[0, 0] - 1] * dmgScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 13)
        {
            //Debug.Log("BossHit");
            health-=playerDamage;         
        }
    }

    private void HealthUpdate()
    {
        healthScale.position = new Vector3(transform.position.x, healthScale.position.y, healthScale.position.z);
        health = Mathf.Clamp(health, 0, 2);
        healthScale.localScale = new Vector2(Mathf.Lerp(healthScale.localScale.x, health, 0.2f), healthScale.localScale.y);
    }

    public IAIContext GetContext(Guid aiId)
    {
        return context;
    }
}
