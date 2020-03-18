using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;
using Apex.Serialization;

[ApexSerializedType, FriendlyName("EyesScan","Ray casts looking for the player and updates context")]
public sealed class Eyes : ActionBase
{  
    public float radius = 20f, timePassed = 0;    //bottom to top
    public float[] rayAngles = new float[] { 35f, 22f, 10f, 0f, -10f, -18f, -26f, -35f, -45f};
    private Transform eyes;
    private Hashtable playerInfo = new Hashtable();
    private List<RaycastHit2D> rays = new List<RaycastHit2D>();
    private Vector3 target;

    public override void Execute(IAIContext context)
    {
        var cont = (AIContext)context;
        eyes = cont.bossEyesTr;
        playerInfo = cont.playerInfo;
        rays.Clear();  

        for (int i = 0; i < rayAngles.Length; i++)
        {
            target = Quaternion.AngleAxis(rayAngles[i], Vector3.forward) * eyes.right;
            Debug.DrawRay(eyes.position, target * radius, Color.green);
            rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));
        }
        
        for (int i = 0; i < rayAngles.Length; i++)
        {         
            if (rays[i])
            {
                playerInfo["position"] = rays[i].collider.gameObject.GetComponent<Transform>().position;
                playerInfo["velocity"] = rays[i].collider.gameObject.GetComponent<Rigidbody2D>().velocity;
                playerInfo["crouch"] = rays[i].collider.gameObject.GetComponent<PlayerController>().crouch;
                timePassed = 0f;
                break;
            }
        }
        timePassed += Time.deltaTime;
        playerInfo["timePassed"] = timePassed;
        //playerInfo["isBehind"] = behind;
        cont.playerInfo = playerInfo;
    }
}
