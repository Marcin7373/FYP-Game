using System.Collections;
using UnityEngine;

public class MissileFollow : MonoBehaviour
{
    public Transform player, boss;
    public float lifetime = 10f;
    private Rigidbody2D rb;
    private ParticleSystem ps;
    private float state, damage = 0.1f;   

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        //transform.Rotate(0,0,180);
        state = 0f;
        StartCoroutine(Destroy(lifetime));
    }

    void Update()
    {
        if (state == 0 && (transform.position - boss.position).magnitude < 20f)
        {
            rb.velocity = transform.up * 4;

            var relativePoint = transform.InverseTransformPoint(player.position);
            if (relativePoint.x < 0.0)
                transform.Rotate(new Vector3(0, 0, 5f));
            else if (relativePoint.x > 0.0)
                transform.Rotate(new Vector3(0, 0, -5f));
        }
        else
        {
            rb.velocity = transform.up * 0; 
            ps.Emit(50);
            StartCoroutine(Destroy(2f));
        }
    }

    IEnumerator Destroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        state = 0;
        gameObject.SetActive(false);
    }

    void SetState()
    {
        state = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && state != 1)
        {
            state = 1;
            Health.Instance.CurHealth += damage;
        }else if (other.gameObject.layer == 8 && state == 0)
        {
            state = 2;
        }
    }
}
