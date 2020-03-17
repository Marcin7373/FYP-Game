using Apex.AI;
using Apex.Serialization;

[ApexSerializedType, FriendlyName("SeePlayer","Scores if player was seen within min interval")]
public class SeePlayer : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("MinTimePassed","Time passed since player last seen")]
    public float minTimePassed = 0.2f;
    [ApexSerialization]
    public float scoreMin = 0f, scoreMax = 1f;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        
        return (float)c.playerInfo["timePassed"] > minTimePassed ? (score = scoreMin) : (score = scoreMax);
    }
}
