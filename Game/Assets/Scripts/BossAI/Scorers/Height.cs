using Apex.AI;
using Apex.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Height : ContextualScorerBase
{
    [ApexSerialization]
    public float minRange = 0, maxRange = 20, scoreMax = 1;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var playerInfo = c.playerInfo;
        float distance = ((Vector3)c.playerInfo["position"] - c.bossTr.position).magnitude;
        return score;
    }
}