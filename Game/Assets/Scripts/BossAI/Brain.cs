﻿using System.Collections;
using UnityEngine;
using Apex.AI.Components;
using Apex.AI;
using System;

public class Brain : MonoBehaviour, IContextProvider
{                                                 //Bite Laser Swipe  TailS  Spike
    private readonly float[] damage = new float[] { 0.2f, 0.16f, 0.1f, 0.1f, 0.1f };
    public Transform eyes, longTail, shortTail;
    public GameObject missile;
    public ParticleSystem laser, shockWave, splash, splashSmall, splashLine;
    public AudioSource[] sfxSrc;
    public AudioClip[] sfxClips;
    private Rigidbody2D rb;
    private Animator anim;
    public Hashtable playerInfo = new Hashtable();
    private AIContext context;             //Current    Bite      Laser     Swipe   TailSwipe  SpikeThrust
    public float[,] history = new float[,] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
    private float dmgScale = 1f, debugTemp, historyIndex;
    private bool dead = false;
    private int sfxIter = 0;

    private void Awake()
    {        
        playerInfo["timePassed"] = 10f;
        playerInfo["position"] = new Vector3(-27, -10, 0);
        playerInfo["velocity"] = new Vector2(0,0);
        playerInfo["crouch"] = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        splashSmall = Instantiate(splashSmall, transform.position, transform.rotation);
        splash = Instantiate(splash, transform.position, transform.rotation);
        splashLine = Instantiate(splashLine, transform.position, transform.rotation);
        context = new AIContext(transform, eyes, rb, anim, false, playerInfo, history, missile);
        debugTemp = history[0,1];
    }

    private void Start()
    {
        Health.Instance.BossPos = transform.position;
    }

    void Update()
    {
        Health.Instance.BossPos = transform.position;
        if (Health.Instance.CurHealth <= 0.01f && !dead && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            anim.SetTrigger("death");
            Health.Instance.CurHealth = 0f;
        }

        if (context.history[0,1] != debugTemp)
        {
            //PrintHistory(context.history);
        }
        debugTemp = context.history[0,1];
        //Debug.Log(Vector3.Distance(transform.position, (Vector3)context.playerInfo["position"]));
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
        longTail.transform.position = new Vector2(((Vector3)context.playerInfo["position"]).x, longTail.transform.position.y);
        splash.transform.position = new Vector3(((Vector3)context.playerInfo["position"]).x, -5.2f, transform.position.z);
        splash.Emit(30);
    }

    void Push() => longTail.transform.position = new Vector2(transform.position.x, longTail.transform.position.y);

    void ShockWave()
    {
        shockWave.Play();
        SFX(7);
    }

    void FlipBoss()
    {
        if (transform.right.x < 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else if (transform.right.x > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }

    void NotBusy() => context.busy = false;

    public void Attacks(int state)
    {
        float baseDamage = damage[(int)context.history[0, 0] - 1] * dmgScale;

        if (state == 1 && context.history[0,0] == 2 && !dead)
        {
            float dis = (Vector3.Distance(transform.position, (Vector3)context.playerInfo["position"]) - 19) / (5f - 19);                        
            context.history[(int)context.history[0, 0], 1] += baseDamage * Time.deltaTime  //base 0.8 * 1.25 = 1 over time 
                    *(Mathf.Clamp(dis * 1.8f, 0, 1)) / (damage[(int)context.history[0, 0] - 1] * 1.25f); //fraction of damage landed
            Health.Instance.CurHealth += baseDamage * Time.deltaTime * dis;                       
        }
        else if (state == 0 && context.history[0, 0] != 2 && !dead && context.history[0, 1] != historyIndex)
        {
            context.history[(int)context.history[0, 0], 1]++;  // times attack hit
            Health.Instance.CurHealth += baseDamage;
            context.history[0, 1]++;    //history index
            historyIndex = context.history[0, 1];
        }
    }

    void Dying()
    {
        dead = true;
        Health.Instance.Dead = true;
        anim.speed = 0f;
    }

    void SFX(int type)
    {
        if (type >= 20)
        {
            if (sfxIter == 0)
            {
                sfxSrc[sfxSrc.Length-1].Stop();
            }
            else
            {
                sfxSrc[sfxIter - 1].Stop();
            }
        }
        else {
            sfxIter = (sfxIter + 1) % sfxSrc.Length;
            sfxSrc[sfxIter].clip = sfxClips[type];
            sfxSrc[sfxIter].Play();
        }
    }

    void Splash(float offset)
    {
        if (offset != 0)
        {
            if (transform.rotation.eulerAngles.y == 0)
            {
                splash.transform.position = new Vector3(transform.position.x - offset, -5.2f, transform.position.z);
            }
            else if (transform.rotation.eulerAngles.y == 180)
            {
                splash.transform.position = new Vector3(transform.position.x + offset, -5.2f, transform.position.z);
            }
            splash.Emit(40);
        }
        else
        {
            splashLine.transform.position = new Vector3(transform.position.x + offset, -4.9f, transform.position.z);
            splashLine.Emit(25);
        }
    }

    public IAIContext GetContext(Guid aiId)
    {
        return context;
    }

    public void DetectCol(Vector3 pos)
    {
        context.playerInfo["position"] = pos;
        context.playerInfo["timePassed"] = -1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && dead)
        {
            dead = false;
            Health.Instance.CurHealth = 1;
            Health.Instance.Dead = false;
            anim.speed = 1f;
        }
    }

    void PrintHistory(float[,] his)
    {
        Debug.Log(his[0, 0] + " | " + his[1, 0] + " | " + his[2, 0] + "    | " + his[3, 0] + " | " + his[4, 0] + " | " + his[5, 0] + "\n \t " +
        "     " + his[1, 1] + " | " + his[2, 1].ToString("F1") + " | " + his[3, 1] + " | " + his[4, 1] + " | " + his[5, 1]);
        //context.history[2, 1] = 0;
    }

    public void SetDmgScale(float dmgScale) => this.dmgScale = dmgScale;
    public bool GetDead() => dead;
}
