using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public class AIContext : IAIContext
{
    public AIContext(Transform transform, Transform eyes, Rigidbody2D rb, Animator anim, float speed, bool busy, Hashtable playerInfo, float[,] history)
    {
        bossTr = transform;
        bossEyesTr = eyes;
        bossRb = rb;
        bossAnim = anim;
        bossSpeed = speed;
        this.busy = busy;
        this.playerInfo = playerInfo;
        this.history = history;
    }

    public Transform bossTr
    {
        get;
        set;
    }

    public Transform bossEyesTr
    {
        get;
        set;
    }

    public Rigidbody2D bossRb
    {
        get;
        set;
    }

    public Animator bossAnim
    {
        get;
        set;
    }

    public float bossSpeed
    {
        get;
        set;
    }

    public bool busy
    {
        get;
        set;
    }

    public Hashtable playerInfo
    {
        get;
        set;
    }

    public float[,] history
    {
        get;
        set;
    }
}
