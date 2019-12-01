using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public sealed class Run : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (c.boss.transform.right.x < 0)
        {
            c.boss.rb.velocity += new Vector2(c.boss.speed, c.boss.rb.velocity.y);
        }
        else if (c.boss.transform.right.x > 0)
        {
            c.boss.rb.velocity += new Vector2(-c.boss.speed, c.boss.rb.velocity.y);
        }
    }
}