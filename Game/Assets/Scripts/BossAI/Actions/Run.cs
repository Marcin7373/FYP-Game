using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public sealed class Run : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;

        float distance = ((Vector3)c.playerInfo["position"] - c.bossTr.position).magnitude;
        if (distance < 6f) {
            //c.boss.rb.velocity += new Vector2(-c.boss.transform.right.x * c.boss.speed, c.boss.rb.velocity.y);
            //c.boss.anim.SetTrigger("swipe");
            
        }
        else
        {
            c.bossRb.velocity += new Vector2(c.bossTr.right.x * c.bossSpeed, c.bossRb.velocity.y);
            
        }
    }
}