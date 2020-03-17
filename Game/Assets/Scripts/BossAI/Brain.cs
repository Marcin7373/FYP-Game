using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI.Components;
using Apex.AI;
using System;

public class Brain : MonoBehaviour, IContextProvider
{                                                 //Bite Laser Swipe TailS
    private readonly float[] damage = new float[] { 0.2f, 0.16f, 0.1f, 0.1f };
    private List<RaycastHit2D> rays = new List<RaycastHit2D>();
    public Transform eyes, longTail, shortTail;
    public ParticleSystem laser;
    private bool faceLeft = true;
    public float speed = 1f, playerDamage = 0.1f, dmgScale = 1f;
    private Rigidbody2D rb;
    private Animator anim;
    public Hashtable playerInfo = new Hashtable();
    private AIContext context;           //Current   Bite      Laser     Swipe    TailSwipe
    public float[,] history = new float[,] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
    private float temp;

    private void Awake()
    {        
        playerInfo["timePassed"] = 10f;
        playerInfo["position"] = new Vector3(-27, -4, 0);
        playerInfo["velocity"] = new Vector2(0,0);
        playerInfo["crouch"] = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        context = new AIContext(this.transform, eyes, rb, anim, speed, false, playerInfo, history);
        temp = history[0,1];
    }

    private void Start()
    {
        Health.Instance.BossPos = transform.position;
    }

    void Update()
    {
        Health.Instance.BossPos = transform.position;
        if (context.history[0,1] != temp)
        {
            PrintHistory(context.history);
        }
        temp = context.history[0,1];
       
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

    void NotBusy() => context.busy = false;

    public void Attacks(int state)
    {
        float baseDamage = damage[(int)context.history[0, 0] - 1] * dmgScale;

        if (state == 1 && context.history[0,0] == 2)
        {
            float dis = (Vector3.Distance(transform.position, (Vector3)context.playerInfo["position"]) - 19) / (5f - 19);                        
            context.history[(int)context.history[0, 0], 1] += baseDamage * Time.deltaTime  //base 0.8 * 1.25 = 1 over time 
                    *(Mathf.Clamp(dis * 1.8f, 0, 1)) / (damage[(int)context.history[0, 0] - 1] * 1.25f); //fraction of damage landed
            Health.Instance.CurHealth += baseDamage * Time.deltaTime * dis;            
            
        }
        else if (state == 0 && context.history[0, 0] != 2)
        {
            context.history[(int)context.history[0, 0], 1]++;  // times attack hit
            Health.Instance.CurHealth += baseDamage;
            //context.history[0, 1]++;    //history index
        }else if (state == 2 && context.history[0,0] == 2)
        {
            //context.history[0, 1]++;    //history index
        }
        //Debug.Log(Vector3.Distance(transform.position, (Vector3)context.playerInfo["position"]));
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

    void PrintHistory(float[,] his)
    {
        Debug.Log(his[0, 0] + " | " + his[1, 0] + " | " + his[2, 0] + "     | " + his[3, 0] + " | " + his[4, 0] + "\n \t " +
        his[0, 1] + " | " + his[1, 1] + " | " + his[2, 1].ToString("F2") + " | " + his[3, 1] + " | " + his[4, 1]);
        //context.history[2, 1] = 0;
    }
}
