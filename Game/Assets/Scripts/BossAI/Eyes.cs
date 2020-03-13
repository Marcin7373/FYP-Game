using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public sealed class Eyes : ActionBase
{  
    public float radius = 20f, timePassed = 0;
    public bool behind = false;
    private Transform eyes;
    public Hashtable playerInfo = new Hashtable();
    private List<RaycastHit2D> rays = new List<RaycastHit2D>();
    private Vector3 target;

    public override void Execute(IAIContext context)
    {
        var cont = (AIContext)context;
        eyes = cont.bossEyesTr;
        playerInfo = cont.playerInfo;
        rays.Clear();
        target = Quaternion.AngleAxis(30.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(15.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(7.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(0.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(-7.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(-15.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(-22.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(-30.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(-45.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));
        
        for (int i = 0; i < 9; i++)
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
        playerInfo["isBehind"] = behind;
        cont.playerInfo = playerInfo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other)
        {
            behind = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other)
        {
            behind = false;
        }
    }
}
