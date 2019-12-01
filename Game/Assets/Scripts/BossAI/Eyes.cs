using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Apex.AI;

public sealed class Eyes : ActionBase
{  
    public float radius = 5f, longRadius = 10f, timePassed = 0;
    public bool behind = false;
    private Transform eyes;
    public Hashtable playerInfo = new Hashtable();
    public List<RaycastHit2D> rays = new List<RaycastHit2D>();

    private void Awake()
    {
        playerInfo["timePassed"] = 0.0f;
        playerInfo["position"] = null;
    }

    public override void Execute(IAIContext context)
    {
        var c = (AIContext)context;
        eyes = c.boss.eyes;
        rays.Clear();
        Vector3 target = Quaternion.AngleAxis(20.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.blue);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(0.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.blue);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(-20.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * radius, Color.blue);
        rays.Add(Physics2D.Raycast(eyes.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(10.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * longRadius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, longRadius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(-10.0f, Vector3.forward) * eyes.right;
        Debug.DrawRay(eyes.position, target * longRadius, Color.green);
        rays.Add(Physics2D.Raycast(eyes.position, target, longRadius, 1 << LayerMask.NameToLayer("Player")));
        
        for (int i = 0; i < 5; i++)
        {         
            if (rays[i])
            {
                playerInfo["position"] = rays[i].collider.gameObject.transform.position;
                playerInfo["velocity"] = rays[i].collider.gameObject.GetComponent<Rigidbody2D>().velocity;
                timePassed = 0;
                break;
            }
            else
            {
                playerInfo["position"] = eyes.position;
            }
        }
        playerInfo["timePassed"] = timePassed;
        playerInfo["isBehind"] = behind;
        timePassed += Time.deltaTime;
        c.playerInfo = playerInfo;
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
