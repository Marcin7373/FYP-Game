using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public sealed class Run : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;

        float distance = ((Vector3)c.playerInfo["position"] - c.boss.transform.position).magnitude;
        if (distance < 6f) {
            c.boss.rb.velocity += new Vector2(-c.boss.transform.right.x * c.boss.speed, c.boss.rb.velocity.y);
            c.boss.sRenderer.sprite = c.boss.redSprite[1];
        }
        else
        {
            c.boss.rb.velocity += new Vector2(c.boss.transform.right.x * c.boss.speed, c.boss.rb.velocity.y);
            c.boss.sRenderer.sprite = c.boss.redSprite[2];
        }
    }
}