using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI.Components;
using Apex.AI;
using System;

public class Brain : MonoBehaviour, IContextProvider
{
    private List<RaycastHit2D> rays = new List<RaycastHit2D>();
    public Transform eyes, longTail, shortTail;
    public ParticleSystem laser;
    private bool faceLeft = true;
    public float speed = 1f;
    private Rigidbody2D rb;
    private Animator anim;
    public Hashtable playerInfo = new Hashtable();
    private AIContext context;

    private void Awake()
    {
        playerInfo["timePassed"] = 10f;
        playerInfo["position"] = new Vector3(-27, -4, 0);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        context = new AIContext(this.transform, eyes, rb, anim, speed, false, playerInfo);
    }

    void Update()
    {
        //rays = eyes.rays;
        //playerInfo = eyes.playerInfo;
        if (rays.Count == 5)
        {
            //SensorResponse();
        }
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

    public IAIContext GetContext(Guid aiId)
    {
        return context;
    }

    void Laser(float on)
    {
        if (on == 1)
        {
            laser.Play();
        }
        else
        {
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
}
