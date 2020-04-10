using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float curHealth;
    private Vector2 bossPos;
    private bool dead;

    public static Health Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        curHealth = transform.localScale.x;
        dead = false;
    }

    void Update()
    {
        transform.position = new Vector3(bossPos.x, transform.position.y, transform.position.z);
        curHealth = Mathf.Clamp(curHealth, 0, 2);

        if ((curHealth > 0 || curHealth < 2f ) && !dead) {
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, curHealth, 0.2f), transform.localScale.y, transform.localScale.z);
        }
        else if(dead)
        {
            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        }
    }

    public float CurHealth { get => curHealth; set => curHealth = value; }
    public Vector2 BossPos { get => bossPos; set => bossPos = value; }
    public bool Dead { get => dead; set => dead = value; }
}
