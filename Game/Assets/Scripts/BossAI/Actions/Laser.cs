using Apex.AI;
using Apex.Serialization;

public class Laser : ActionBase
{
    private const int ID = 2;
    [ApexSerialization, FriendlyName("Interval","interval between attacking and failing e.g.(4) attacks 4 times then fails 4 times")]
    public float interval = 4;

    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {
            if ((c.history[ID, 0] % (interval * 2)) / interval <= 0.5f)
            {
                c.bossAnim.SetTrigger("laser");
            }
            else
            {
                c.bossAnim.SetTrigger("laserFail");
            }

            c.busy = true;
            c.history[0,0] = ID;  //current
            c.history[0, 1]++;    //history index
            c.history[ID,0]++;    //times used           
        }
    }
}