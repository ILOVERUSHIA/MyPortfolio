using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform shootPoint;
    [SerializeField] public float shootCooldown = 0.3f;
    [SerializeField] public float bulletSpeed = 10f;
    
    private PlayerController playerController;
    private float shootCooldownTimer;
    
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab not assigned!");
        }
        
        // If no shoot point assigned, use player position
        if (shootPoint == null)
        {
            shootPoint = transform;
        }
    }
    
    private void Update()
    {
        if (shootCooldownTimer > 0f)
        {
            shootCooldownTimer -= Time.deltaTime;
        }
        
        // Shoot when Z key is pressed
        if (Input.GetKeyDown(KeyCode.Z) && shootCooldownTimer <= 0f)
        {
            Shoot();
        }
    }
    
    private void Shoot()
    {
        if (bulletPrefab == null)
            return;
        
        // Instantiate bullet at shoot point
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        
        // Get bullet component and set direction
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            int direction = playerController.GetFacingDirection();
            bulletComponent.SetDirection(direction);
            bulletComponent.SetSpeed(bulletSpeed);
        }
        else
        {
            // Fallback: add velocity directly
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = new Vector2(bulletSpeed * playerController.GetFacingDirection(), 0f);
            }
        }
        
        shootCooldownTimer = shootCooldown;
    }
}
