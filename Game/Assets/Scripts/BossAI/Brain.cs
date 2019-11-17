using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    private List<RaycastHit2D> rays = new List<RaycastHit2D>();
    public Eyes eyes;
    private bool faceLeft = true;
    public Sprite[] redSprite = new Sprite[3];
    public float speed = 1f;
    private SpriteRenderer sRenderer;
    private Rigidbody2D rb;
    private Hashtable playerInfo = new Hashtable();

    UAI_Agent agent;
    public UAI_PropertyBoundedFloat playerDistance, timeUndetected;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();    
    }

    void Start()
    {
        agent = GetComponent<UAI_Agent>();
        agent.SetVoidActionDelegate("Run", Run);
        agent.SetVoidActionDelegate("Look", Look);
    }

    void Update()
    {
        rays = eyes.rays;
        playerInfo = eyes.playerInfo;
        if (rays.Count == 5)
        {
            //SensorResponse();
        }

        if (playerInfo["position"] != null && (float)playerInfo["timePassed"] < 1f)
        {
            playerDistance.value = (transform.position - (Vector3)playerInfo["position"]).magnitude;
        } else
        {
            playerDistance.value = 10f;
        }

        if ((float)playerInfo["timePassed"] < 2f) {
            timeUndetected.value = (float)playerInfo["timePassed"];
        }
        else
        {
            timeUndetected.value = 2f;
        }
        agent.UpdateAI();
    }

    void Run()
    {
        if (faceLeft)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else if (!faceLeft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }

    void Look()
    {
        if (faceLeft)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            faceLeft = false;
        }
        else if (!faceLeft)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            faceLeft = true;
        }
    }

    private void SensorResponse()
    {        
        if ((bool)playerInfo["isBehind"] && faceLeft)
        { 
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            eyes.behind = false;
            faceLeft = false;
        }
        else if ((bool)playerInfo["isBehind"] && !faceLeft)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            eyes.behind = false;
            faceLeft = true;
        }

        if (rays[0] && rays[1] && rays[2] && faceLeft)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }else if (rays[0] && rays[1] && rays[2] && !faceLeft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
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
