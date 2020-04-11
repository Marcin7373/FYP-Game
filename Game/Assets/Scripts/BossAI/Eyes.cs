using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;
using Apex.Serialization;

[ApexSerializedType, FriendlyName("EyesScan","Ray casts looking for the player and updates context")]
public sealed class Eyes : ActionBase
{  
    [ApexSerialization]
    public float radius = 21f;    
    [ApexSerialization]                   //bottom to top
    public float[] rayAngles = new float[] { 37f, 32f, 26f, 20f, 14f, 8f, 2f, -5f, -12f, -19f, -25f, -30f, -37f, -45f, -50f};
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
            //Debug.DrawRay(eyes.position, target * radius, Color.green);
            rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));
        }
        
        for (int i = 0; i < rayAngles.Length; i++)
        {     
            if (rays[i])
            {
                playerInfo["position"] = rays[i].collider.gameObject.GetComponent<Transform>().position;
                playerInfo["velocity"] = rays[i].collider.gameObject.GetComponent<Rigidbody2D>().velocity;
                playerInfo["crouch"] = rays[i].collider.gameObject.GetComponent<PlayerController>().crouch;
                playerInfo["timePassed"] = 0f;
                break;
            }
        }

        playerInfo["timePassed"] = (float)playerInfo["timePassed"] + Time.deltaTime;
        cont.playerInfo = playerInfo;
    }
}
