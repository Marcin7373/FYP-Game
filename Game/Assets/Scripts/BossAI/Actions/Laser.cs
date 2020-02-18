using Apex.AI;

public class Laser : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {
            c.bossAnim.SetTrigger("laser");
            c.busy = true;
        }
    }
}