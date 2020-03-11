using UnityEngine;

public class AttackCol : MonoBehaviour
{
    private Brain brain;
    void Awake() => brain = transform.parent.GetComponent<Brain>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            brain.Attacks(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            brain.Attacks(true);
        }
    }
}
