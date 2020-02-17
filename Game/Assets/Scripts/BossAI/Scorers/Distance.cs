using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public class Distance : ContextualScorerBase
{
    [ApexSerialization]
    public float minRange = 0, maxRange = 20, scoreMax = 1;
    private float distance, mid;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var playerInfo = c.playerInfo;
        distance = ((Vector3)c.playerInfo["position"] - c.bossTr.position).magnitude;
        mid = (maxRange - minRange) / 2 + minRange;

        if (distance >= mid  && distance <= maxRange)
        { 
            score = 1 + Mathf.Abs((distance - maxRange) * (scoreMax - 1) / (mid - maxRange));
        }
        else if (distance >= minRange && distance <= maxRange)
        {
            score = 1 + ((distance - minRange) * (scoreMax - 1) / (mid - minRange));
        }
        else
        {
            score = 0;
        }

        //Debug.Log(distance+" "+ score);
        return score;
    }
}