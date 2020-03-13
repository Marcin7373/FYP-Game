using Apex.AI;

public class SeePlayer : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
 
        return (float)c.playerInfo["timePassed"] > 0.2f ? (score = 0f) : (score = 1f);
    }
}
