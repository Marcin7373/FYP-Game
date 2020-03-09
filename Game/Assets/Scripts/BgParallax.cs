using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgParallax : MonoBehaviour
{
    public Transform[] backgrounds;
    public float smoothing = 1f;
    private float bgTargetPosX, bgTargetPosY;

    private Transform cam;
    private Vector3 previousCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            bgTargetPosX = backgrounds[i].position.x + ((previousCamPos.x - cam.position.x) * -backgrounds[i].position.z);

            if (i > 9) {
                bgTargetPosY = backgrounds[i].position.y + ((previousCamPos.y - cam.position.y) * -backgrounds[i].position.z);
            }
            else
            {
                bgTargetPosY = backgrounds[i].position.y;
            }
            Vector3 backgroundTargetPos = new Vector3(bgTargetPosX, bgTargetPosY, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
