using Apex.AI;

public class TailSwipe : ActionBase
{
    private const int ID = 4;
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {
            c.bossAnim.SetTrigger("tailSwipe");
            c.busy = true;
            c.history[0,0] = ID;  //current
            c.history[ID,0]++;    //times used
        }
    }
}