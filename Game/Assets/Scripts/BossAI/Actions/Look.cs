using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public sealed class Look : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (c.boss.transform.right.x < 0)
        {
            c.boss.transform.rotation = Quaternion.Euler(c.boss.transform.rotation.x, 0, c.boss.transform.rotation.z);
        }
        else if (c.boss.transform.right.x > 0)
        {
            c.boss.transform.rotation = Quaternion.Euler(c.boss.transform.rotation.x, 180, c.boss.transform.rotation.z);
        }

        c.boss.sRenderer.sprite = c.boss.redSprite[0];
    }
}
