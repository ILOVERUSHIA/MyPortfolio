using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float lifetime = 5f;
    [SerializeField] public float speed = 10f;
    
    private Rigidbody2D rb;
    private int direction = 1;
    private float lifeTimer;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeTimer = lifetime;
        
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(speed * direction, 0f);
        }
    }
    
    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
    
    public void SetDirection(int newDirection)
    {
        direction = newDirection > 0 ? 1 : -1;
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy bullet on collision with anything except the player
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Also destroy on regular collision
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
