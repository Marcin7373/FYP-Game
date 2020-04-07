using Apex.AI;

public class Push : ActionBase
{
    private const int ID = 8;

    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {
            c.bossAnim.SetTrigger("push");
            c.history[0, 0] = ID;  //current
            c.history[0, 1]++;    //history index
            c.busy = true;
        }   
    }
}
