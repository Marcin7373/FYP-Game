using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public class AIContext : IAIContext
{
    public AIContext(Brain boss)
    {
        this.boss = boss;
        this.playerInfo = playerInfo;
    }

    public Brain boss
    {
        get;
        private set;
    }

    public Hashtable playerInfo
    {
        get;
        set;
    }


}
