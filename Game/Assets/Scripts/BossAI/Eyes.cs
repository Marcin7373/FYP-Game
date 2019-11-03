using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{  
    public float radius = 5f;
    public float longRadius = 10f;
    public bool behind = false;
    public List<RaycastHit2D> rays = new List<RaycastHit2D>();

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

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other)
        {
            behind = true;
            Debug.Log("behind");
        }
    }
}
