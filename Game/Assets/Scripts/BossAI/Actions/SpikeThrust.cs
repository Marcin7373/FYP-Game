using Apex.AI;

public class SpikeThrust : ActionBase
{
    private const int ID = 5;
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {
            c.bossAnim.SetTrigger("spikeThrust");
            c.busy = true;
            c.history[0, 0] = ID;  //current
            c.history[0, 1]++;    //history index
            c.history[ID, 0]++;    //times used          
        }
    }
}
