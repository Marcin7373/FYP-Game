using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTarget, bossTarget;
    public float smoothing;
    private Vector3 temp;

    void LateUpdate()
    {
        if (Vector3.Distance(playerTarget.position, bossTarget.position) < 20)
        {
            if(playerTarget.position.x < bossTarget.position.x - 4){
                temp = new Vector3(Mathf.Clamp(bossTarget.position.x-10, -6f, 25f), 
                    Mathf.Clamp(playerTarget.position.y, -1.24f, 1.3f), transform.position.z);
            }
            else if(playerTarget.position.x > bossTarget.position.x + 4)
            {
                temp = new Vector3(bossTarget.position.x+10, 
                    Mathf.Clamp(playerTarget.position.y, -1.24f, 1.3f), transform.position.z);
            }
            
            transform.position = Vector3.Lerp(transform.position, temp, smoothing * Time.deltaTime);           
        }
        else
        {
            temp = new Vector3(Mathf.Clamp(playerTarget.position.x, -16f, 36f), Mathf.Clamp(playerTarget.position.y, -1.24f, 1.3f), transform.position.z);
            transform.position = Vector3.Lerp(transform.position, temp, smoothing * Time.deltaTime);
        }       
    }
}
