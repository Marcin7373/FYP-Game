﻿using System.Collections;
using UnityEngine;
using Apex.AI;

public class AIContext : IAIContext
{
    public AIContext(Transform transform, Transform eyes, Rigidbody2D rb, Animator anim, bool busy, Hashtable playerInfo, float[,] history, GameObject missile)
    {
        bossTr = transform;
        bossEyesTr = eyes;
        bossRb = rb;
        bossAnim = anim;
        this.busy = busy;
        this.playerInfo = playerInfo;
        this.history = history;
        this.missile = missile;
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

    public GameObject missile
    {
        get;
        set;
    }
}
