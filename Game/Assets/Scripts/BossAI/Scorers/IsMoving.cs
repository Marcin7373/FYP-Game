using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public class IsMoving : ContextualScorerBase
{
    [ApexSerialization]
    public float maxSpeed, minSpeed, scoreMin = 1, scoreMax = 1;
    private float velocity;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        velocity = ((Vector2)c.playerInfo["velocity"]).x;

        return score = scoreMin + ((Mathf.Abs(velocity) - maxSpeed) * (scoreMax - scoreMin) / (minSpeed - maxSpeed));
    }
}