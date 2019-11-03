using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    private List<RaycastHit2D> rays = new List<RaycastHit2D>();
    public Eyes eyes;
    private bool faceLeft = true;
    public Sprite[] redSprite = new Sprite[3];
    public float speed = 0.5f;
    private SpriteRenderer sRenderer;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rays = eyes.rays;
        if (rays.Count == 5)
        {
            SensorResponse();
        }

        if (eyes.behind && faceLeft)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            faceLeft = false;
        }
        else if (eyes.behind && !faceLeft)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            faceLeft = true;
        }

    }

    private void SensorResponse()
    {
        if (rays[0] && rays[1] && rays[2] && faceLeft)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        
        if (eyes.behind && faceLeft)
        { 
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            eyes.behind = false;
            faceLeft = false;
        }
        else if (eyes.behind && !faceLeft)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            eyes.behind = false;
            faceLeft = true;
        }

        if (rays[3] || rays[4])
        {
            sRenderer.sprite = redSprite[2];
        }

        if (rays[0] || rays[1] || rays[2])
        {
            sRenderer.sprite = redSprite[1];
        }
        else if (!(rays[0] || rays[1] || rays[2] || rays[3] || rays[4]))
        {
            sRenderer.sprite = redSprite[0];
        }
    } 
}
