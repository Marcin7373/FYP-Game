using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{  
    public float radius = 5f, longRadius = 10f, timePassed = 0;
    public bool behind = false;
    public Hashtable playerInfo = new Hashtable();
    public List<RaycastHit2D> rays = new List<RaycastHit2D>();

    private void Awake()
    {
        playerInfo["timePassed"] = 0.0f;
        playerInfo["position"] = null;
    }

    void Update()
    {
        rays.Clear();
        Vector3 target = Quaternion.AngleAxis(20.0f, Vector3.forward) * transform.right;
        Debug.DrawRay(transform.position, target * radius, Color.blue);
        rays.Add(Physics2D.Raycast(transform.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(0.0f, Vector3.forward) * transform.right;
        Debug.DrawRay(transform.position, target * radius, Color.blue);
        rays.Add(Physics2D.Raycast(transform.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(-20.0f, Vector3.forward) * transform.right;
        Debug.DrawRay(transform.position, target * radius, Color.blue);
        rays.Add(Physics2D.Raycast(transform.position, target, radius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(10.0f, Vector3.forward) * transform.right;
        Debug.DrawRay(transform.position, target * longRadius, Color.green);
        rays.Add(Physics2D.Raycast(transform.position, target, longRadius, 1 << LayerMask.NameToLayer("Player")));

        target = Quaternion.AngleAxis(-10.0f, Vector3.forward) * transform.right;
        Debug.DrawRay(transform.position, target * longRadius, Color.green);
        rays.Add(Physics2D.Raycast(transform.position, target, longRadius, 1 << LayerMask.NameToLayer("Player")));
        
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
                playerInfo["position"] = transform.position;
            }
        }
        playerInfo["timePassed"] = timePassed;
        playerInfo["isBehind"] = behind;
        timePassed += Time.deltaTime;
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
