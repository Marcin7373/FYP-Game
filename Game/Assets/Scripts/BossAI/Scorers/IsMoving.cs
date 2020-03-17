using Apex.AI;
using Apex.Serialization;
using UnityEngine;

[ApexSerializedType, FriendlyName("IsMoving","Increases score if player is not moving and 0 if at max speed")]
public class IsMoving : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("MaxSpeed","Top speed as velocity on x axis")]
    public float maxSpeed;
    [ApexSerialization]
    public float minSpeed, scoreMin = 0, scoreMax = 1;
    private float velocity;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        velocity = ((Vector2)c.playerInfo["velocity"]).x;

        return score = scoreMin + ((Mathf.Abs(velocity) - maxSpeed) * (scoreMax - scoreMin) / (minSpeed - maxSpeed));
    }
}