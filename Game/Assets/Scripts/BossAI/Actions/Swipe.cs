using Apex.AI;

public class Swipe : ActionBase
{
    private const int ID = 3;
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {            
            c.bossAnim.SetTrigger("swipe");
            c.busy = true;
            c.history[0,0] = ID;  //current
            c.history[ID,0]++;    //times used
        } 
    }
}
