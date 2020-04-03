using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTarget, playerPos, bossTarget;
    public float smoothing;
    private Vector3 temp;

    void LateUpdate()
    {
        if (Vector3.Distance(playerPos.position, bossTarget.position) < 20)
        {
            if(playerPos.position.x < bossTarget.position.x - 2){ //left of boss
                temp = new Vector3(bossTarget.position.x-10, 
                    Mathf.Clamp(playerTarget.position.y, -1.24f, 1.3f), transform.position.z);
            }
            else if(playerPos.position.x > bossTarget.position.x + 2)//right of boss
            {
                temp = new Vector3(bossTarget.position.x+10, 
                    Mathf.Clamp(playerTarget.position.y, -1.24f, 1.3f), transform.position.z);
            }
            
            transform.position = Vector3.Lerp(transform.position, temp, smoothing * Time.deltaTime);           
        }
        else
        {
            temp = new Vector3(Mathf.Clamp(playerTarget.position.x, -15f, 35f), Mathf.Clamp(playerTarget.position.y, -1.24f, 1.3f), transform.position.z);
            transform.position = Vector3.Lerp(transform.position, temp, smoothing * Time.deltaTime);
        }       
    }
}
