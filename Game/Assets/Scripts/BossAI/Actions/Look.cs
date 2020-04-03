using UnityEngine;
using Apex.AI;

public sealed class Look : ActionBase
{
    private const int ID = 6;

    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;

        if (!c.busy)
        {
            c.bossAnim.SetTrigger("look");
            c.busy = true;
            c.history[0, 0] = ID;
            c.history[0, 1]++;    //history index
        }        
    }
}
