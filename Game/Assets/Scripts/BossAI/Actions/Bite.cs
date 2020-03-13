using Apex.AI;

public class Bite : ActionBase
{
    private const int ID = 1;
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {
            c.bossAnim.SetTrigger("bite");
            c.busy = true;
            c.history[0,0] = ID;  //current
            c.history[ID,0]++;    //times used
        }   
    }
}
