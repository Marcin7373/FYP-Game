using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing;

    void LateUpdate()
    {
        target.position = new Vector3(Mathf.Clamp(target.position.x, -17f, 15f), Mathf.Clamp(target.position.y, -1.24f, 1.3f), transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
    }
}
