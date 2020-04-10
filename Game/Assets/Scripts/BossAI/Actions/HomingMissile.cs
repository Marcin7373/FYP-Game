using Apex.AI;
using UnityEngine;

public class HomingMissile : ActionBase
{
    private const int ID = 9;

    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        if (!c.busy)
        {            
            c.bossAnim.SetTrigger("homingMissile");
            c.busy = true;
            c.history[0,0] = ID;  //current
            c.history[0, 1]++;    //history index

            if (c.bossTr.rotation.y == 0)
            {
                c.missile.transform.position = new Vector3(c.bossTr.position.x - 3.34f, c.bossTr.position.y + 1.46f, 0);
            }
            else if (c.bossTr.rotation.y == 1)
            {
                c.missile.transform.position = new Vector3(c.bossTr.position.x + 3.34f, c.bossTr.position.y + 1.46f, 0);
            }
            
            c.missile.SetActive(true);
        } 
    }
}
