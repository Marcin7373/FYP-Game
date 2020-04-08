using Apex.AI;
using Apex.Serialization;

[ApexSerializedType, FriendlyName("IsActive", " If object is active returns newScore if not returns 0")]
public class IsActive : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("Not", "If true its reversed, e.g. true -> player crouching = 0")]
    public bool not = false;
    [ApexSerialization]
    public float newScore = -50f;
    private bool mActive; 

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        mActive = c.missile.activeSelf;

        if (not)
        {
            mActive = !mActive;
        }

        if (mActive)
        {
            return score = newScore;
        }
        else
        {
            return score = 0f;
        }
    }
}