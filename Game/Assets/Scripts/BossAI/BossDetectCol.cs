using UnityEngine;

public class BossDetectCol : MonoBehaviour
{
    private Brain brain;
    void Awake() => brain = transform.parent.GetComponent<Brain>();

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 9)
        {
            brain.DetectCol(new Vector3(other.GetContact(0).point.x, other.GetContact(0).point.y, 0));
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 9)
        {
            brain.DetectCol(new Vector3(other.GetContact(0).point.x, other.GetContact(0).point.y, 0));
        }
    }
}
