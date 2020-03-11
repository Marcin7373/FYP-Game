using Apex.AI;

public class TailSwipe : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {
            c.bossAnim.SetTrigger("tailSwipe");
            c.busy = true;
            c.attack = 0.1f;
        }
    }
}