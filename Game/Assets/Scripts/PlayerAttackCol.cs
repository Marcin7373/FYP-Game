using UnityEngine;

public class PlayerAttackCol : MonoBehaviour
{
    private PlayerController player;
    void Awake() => player = transform.parent.GetComponent<PlayerController>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            player.Attack();
            player.attackHitEffect.Play();
            player.hitSfx[1].Play();
        }
    }
}
