using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public float smoothSpeed = 0.125f;
    [SerializeField] public Vector3 offset = new Vector3(0f, 1f, -10f);
    [SerializeField] public float minX = -10f;
    [SerializeField] public float maxX = 10f;
    [SerializeField] public float minY = -5f;
    [SerializeField] public float maxY = 5f;
    
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }
    
    private void LateUpdate()
    {
        if (player == null)
            return;
        
        Vector3 desiredPosition = player.position + offset;
        
        // Clamp the camera position within bounds
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        
        // Smoothly move camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
