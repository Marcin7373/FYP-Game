using UnityEngine;
using Apex.AI;

public sealed class Look : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        float timeout = 0;

        if (!c.busy)
        {
            c.bossAnim.SetTrigger("look");
        }        
    }
}
