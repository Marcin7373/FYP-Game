using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI.Components;
using Apex.AI;
using System;

public class Brain : MonoBehaviour, IContextProvider
{
    private List<RaycastHit2D> rays = new List<RaycastHit2D>();
    //public Eyes eyes;
    public Transform eyes;
    private bool faceLeft = true;
    public Sprite[] redSprite = new Sprite[3];
    public float speed = 1f;
    public SpriteRenderer sRenderer;
    public Rigidbody2D rb;
    public Hashtable playerInfo = new Hashtable();
    private AIContext context;

    private void Awake()
    {
        eyes = transform.GetChild(0);
        context = new AIContext(this);
        playerInfo["timePassed"] = 10f;
        context.playerInfo = playerInfo;
        rb = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
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

        if (rays[3] || rays[4])
        {
            sRenderer.sprite = redSprite[2];
        }

        if (rays[0] || rays[1] || rays[2])
        {
            sRenderer.sprite = redSprite[1];
        }
        else if (!(rays[0] || rays[1] || rays[2] || rays[3] || rays[4]))
        {
            sRenderer.sprite = redSprite[0];
        }
    }

    public IAIContext GetContext(Guid aiId)
    {
        return context;
    }
}
