using UnityEngine;
using Apex.AI;

public sealed class Look : ActionBase
{
    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        float timeout = 0;

        if (!c.busy)
        {
            c.bossAnim.SetTrigger("look");

            while (!c.bossAnim.GetCurrentAnimatorStateInfo(0).IsName("Look") && timeout < 3f)
            {
                if (c.bossTr.right.x < 0)
                {
                    c.bossTr.rotation = Quaternion.Euler(c.bossTr.rotation.x, 0, c.bossTr.rotation.z);
                    break;
                }
                else if (c.bossTr.right.x > 0)
                {
                    c.bossTr.rotation = Quaternion.Euler(c.bossTr.rotation.x, 180, c.bossTr.rotation.z);
                    break;
                }
                timeout += Time.deltaTime;
            }           
        }        
    }
}
