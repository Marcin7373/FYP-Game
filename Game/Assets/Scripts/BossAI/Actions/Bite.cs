using Apex.AI;

public class Bite : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {
            c.bossAnim.SetTrigger("bite");
            c.busy = true;
            c.attack = 0.2f;
        }   
    }
}
