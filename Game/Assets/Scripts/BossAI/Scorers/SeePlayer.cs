using Apex.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePlayer : ContextualScorerBase
{
    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        var playerInfo = c.playerInfo;
        
        if ((float)playerInfo["timePassed"] > 0.2f)
        {
            return score = 0f;
        }
        return score = 1f;
    }
}
