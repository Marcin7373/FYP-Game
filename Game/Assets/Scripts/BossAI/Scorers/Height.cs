using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public class Height : ContextualScorerBase
{
    [ApexSerialization]
    public float maxPosY = 4.2f, minPosY = -2.85f, scoreMin = 1, scoreMax = 1;
    private float ypos,  scoreMid;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        ypos = ((Vector3)c.playerInfo["position"]).y;
        scoreMid = (scoreMax - scoreMin) / 2 + scoreMin;

        if (((Vector2)c.playerInfo["velocity"]).y >= 0 && ypos > minPosY)
        {
            score = scoreMin + ((ypos - minPosY) * (scoreMid - scoreMin) / (maxPosY - minPosY));
        }
        else if (((Vector2)c.playerInfo["velocity"]).y < 0 && ypos > minPosY)
        {
            score = scoreMid + ((ypos - maxPosY) * (scoreMax - scoreMid) / (minPosY - maxPosY));
        }
        else
        {
            score = 0;
        }
        //Debug.Log(score + " "+ ypos+ " "+ ((Vector2)c.playerInfo["velocity"]).y);

        return score;
    }
}