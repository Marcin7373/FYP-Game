using Apex.AI;
using Apex.Serialization;
using UnityEngine;

[ApexSerializedType, FriendlyName("OnGround", "Not same as players grounded bool, checks if on floor height")]
public class OnGround : ContextualScorerBase
{
    [ApexSerialization]
    public float floorPosY = -3.07f, scoreMin = 0, scoreMax = 10;
    [ApexSerialization, FriendlyName("Not", "If true its reversed, e.g. true -> player grounded = 0")]
    public bool not = false;
    private float posY;
    private bool grounded = false;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;
        posY = ((Vector3)c.playerInfo["position"]).y;

        if (floorPosY + 0.2f > posY)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (not)
        {
            grounded = !grounded;
        }

        if (grounded)
        {
            return score = scoreMax;
        }
        else
        {
            return score = scoreMin;
        }
              
    }
}