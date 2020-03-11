using Apex.AI;
using Apex.Serialization;

public class IsCrouching : ContextualScorerBase
{
    [ApexSerialization]
    public bool not = false;
    private bool crouch; 

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var playerInfo = c.playerInfo;
        crouch = (bool)c.playerInfo["crouch"];

        if (not)
        {
            crouch = !crouch;
        }

        if (crouch)
        {
            return score = 0;
        }
        else if((float)c.playerInfo["timePassed"] < 0.5f)
        {
            return score = 0.1f;
        }
        else
        {
            return score = 0f;
        }
    }
}