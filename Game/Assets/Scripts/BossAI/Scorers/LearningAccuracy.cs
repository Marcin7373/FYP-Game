using Apex.AI;
using Apex.Serialization;
using UnityEngine;

public class LearningAccuracy : ContextualScorerBase
{
    [ApexSerialization]
    public float scoreMin = 0, scoreMax = 10;
    [ApexSerialization, FriendlyName("ID","ID of attack to check accuracy if ID = 0 returns score = 0")]
    public int ID = 0;
    [ApexSerialization, FriendlyName("MinFrequency", "Will look at history of attacks above frequency eg. If (3) accuracy modifications applied on 4th attack")]
    public int minFrequency = 3;

    public override float Score(IAIContext context)
    {
        var c = (AIContext)context;

        if (ID == 0)
        {
            score = 0;
        }else if (c.history[ID,0] <= minFrequency || c.history[0,1] / (c.history.Length-1) <= minFrequency)
        {
            score = ((scoreMax - scoreMin) / 2) + scoreMin;
        }else
        {
            score = (c.history[ID, 1] / c.history[ID, 0]) * (scoreMax - scoreMin) + scoreMin;
        }
        
        return score;
    }
}
