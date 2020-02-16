using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public sealed class Look : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (c.bossTr.right.x < 0)
        {
            c.bossTr.rotation = Quaternion.Euler(c.bossTr.rotation.x, 0, c.bossTr.rotation.z);
        }
        else if (c.bossTr.right.x > 0)
        {
            c.bossTr.rotation = Quaternion.Euler(c.bossTr.rotation.x, 180, c.bossTr.rotation.z);
        }
    }
}
