using Apex.AI;

public class Swipe : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {            
            c.bossAnim.SetTrigger("swipe");
            c.busy = true;
            c.attack = 0.1f;
        } 
    }
}
