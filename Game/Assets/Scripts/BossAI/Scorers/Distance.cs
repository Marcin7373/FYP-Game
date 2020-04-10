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
        if ((c.bossTr.position.x - 1 >= ((Vector3)c.playerInfo["position"]).x && c.bossTr.rotation.y == 0) || (c.bossTr.position.x + 1 < ((Vector3)c.playerInfo["position"]).x && c.bossTr.rotation.y == 1))
        {
            distance = ((Vector3)c.playerInfo["position"] - c.bossTr.position).magnitude;
        }
        else
        {
            distance = -30f;
        }
        

        mid = (maxRange - minRange) / 2 + minRange;

        if (distance >= mid  && distance <= maxRange)
        { 
            return score = scoreMin + ((distance - maxRange) * (scoreMax - scoreMin) / (mid - maxRange));
        }
        else if (distance >= minRange && distance <= maxRange)
        {
            return score = scoreMin + ((distance - minRange) * (scoreMax - scoreMin) / (mid - minRange));
        }
        else if (distance > maxRange+3 || distance < minRange-3)
        {
            return score = -20f;
        }
        else
        {
            return score = 0;
        }
    }
}