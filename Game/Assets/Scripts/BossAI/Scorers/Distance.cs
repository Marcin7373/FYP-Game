using Apex.AI;
using Apex.Serialization;
using UnityEngine;

[ApexSerializedType, FriendlyName("Distance","Middle of range being the max score tapering off to 0 to both edges/ranges")]
public class Distance : ContextualScorerBase
{
    [ApexSerialization]
    public float minRange = 0, maxRange = 20, scoreMin = 0, scoreMax = 10;
    private float distance, mid;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        distance = ((Vector3)c.playerInfo["position"] - c.bossTr.position).magnitude;
        mid = (maxRange - minRange) / 2 + minRange;

        if (distance >= mid  && distance <= maxRange)
        { 
            return score = scoreMin + ((distance - maxRange) * (scoreMax - scoreMin) / (mid - maxRange));
        }
        else if (distance >= minRange && distance <= maxRange)
        {
            return score = scoreMin + ((distance - minRange) * (scoreMax - scoreMin) / (mid - minRange));
        }
        else
        {
            return score = 0;
        }
    }
}