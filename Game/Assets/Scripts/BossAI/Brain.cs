using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI.Components;
using Apex.AI;
using System;

public class Brain : MonoBehaviour, IContextProvider
{                                        //Bite Laser Swipe  TailS
    private readonly float[] damage = new float[] { 0.2f, 0.1f, 0.1f, 0.1f };
    //private Health healthObj = new Health();
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
        Health.Instance.BossPos = transform.position;
        playerInfo["timePassed"] = 10f;
        playerInfo["position"] = new Vector3(-27, -4, 0);
        playerInfo["velocity"] = new Vector2(0,0);
        playerInfo["crouch"] = false;
        //health = healthScale.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        context = new AIContext(this.transform, eyes, rb, anim, speed, false, playerInfo, history);
    }

    void Update()
    {
        Health.Instance.BossPos = transform.position;

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
            Health.Instance.CurHealth+=(damage[context.history[0, 0]-1] * Time.deltaTime) * dmgScale;
        }
        else if (!col)
        {
            context.history[(context.history[0, 0]), 1]++;  // times attack hit
            Health.Instance.CurHealth += damage[context.history[0, 0] - 1] * dmgScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 14)
        {
            //Debug.Log("BossHit");
            Health.Instance.CurHealth-=playerDamage;         
        }
    }

    public IAIContext GetContext(Guid aiId)
    {
        return context;
    }
}
