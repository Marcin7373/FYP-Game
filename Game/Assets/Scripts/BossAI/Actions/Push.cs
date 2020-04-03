using Apex.AI;
using UnityEngine;

public class Push : ActionBase
{
    private const int ID = 7;
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {
            c.bossAnim.SetTrigger("push");
            c.playerInfo["timePassed"] = 0f;
            c.history[0, 0] = ID;  //current
            c.history[0, 1]++;    //history index
            c.busy = true;
        }   
    }
}
