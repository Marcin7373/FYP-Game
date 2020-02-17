using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public class Bite : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        //c.bossAnim.SetTrigger("bite");
    }
}
