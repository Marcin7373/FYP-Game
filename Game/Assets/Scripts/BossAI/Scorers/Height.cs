using Apex.AI;
using Apex.Serialization;
using UnityEngine;

[ApexSerializedType, FriendlyName("Height", "Increases score across a jump, zero score then up to half score at peak and max score before landing")]
public class Height : ContextualScorerBase
{
    [ApexSerialization]
    public float maxPosY = 4.2f, minPosY = -2.85f, scoreMin = 0, scoreMax = 10;
    private float posY, scoreMid;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        posY = ((Vector3)c.playerInfo["position"]).y;
        scoreMid = (scoreMax - scoreMin) / 2 + scoreMin;

        if (((Vector2)c.playerInfo["velocity"]).y >= 0 && posY > minPosY)
        {
            score = scoreMin + ((posY - minPosY) * (scoreMid - scoreMin) / (maxPosY - minPosY));
        }
        else if (((Vector2)c.playerInfo["velocity"]).y < 0 && posY > minPosY)
        {
            score = scoreMid + ((posY - maxPosY) * (scoreMax - scoreMid) / (minPosY - maxPosY));
        }
        else
        {
            score = 0;
        }
        //Debug.Log(score + "  "+ ypos+ "  "+ ((Vector2)c.playerInfo["velocity"]).y);

        return score;
    }
}