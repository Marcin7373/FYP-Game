using Apex.AI;
using Apex.Serialization;
using UnityEngine;

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

        return ((c.bossTr.position.x+1 >= ((Vector3)c.playerInfo["position"]).x && c.bossTr.rotation.y == 1) || (c.bossTr.position.x-1 < ((Vector3)c.playerInfo["position"]).x && c.bossTr.rotation.y == 0))
            ? (score = scoreMin)
            : (score = scoreMax);
    }
}
