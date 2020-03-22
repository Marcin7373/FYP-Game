using Apex.AI;
using Apex.Serialization;

[ApexSerializedType, FriendlyName("IsCrouching", "By default if crouching returns 0 if not returns score set")]
public class IsCrouching : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Not", "If true its reversed, e.g. true -> player crouching = 0")]
    public bool not = false;
    private bool crouch; 

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
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
            return score;
        }
        else
        {
            return score = 0f;
        }
    }
}