using Apex.AI;
using Apex.Serialization;
using UnityEngine;

[ApexSerializedType, FriendlyName("RepeatAfter", "Takes ID of attack and returns set score if last attack used was the ID, 0 if no match")]
public class NotRepeat : ContextualScorerBase
{
    [ApexSerialization, FriendlyName("ID", "If ID of last attack matches score is set to given score")]
    public int ID;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;

        if (c.history[0,0] == ID)
        {
            return score;
        }
        else
        {
            return score = 0;
        }
    }
}