using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 3f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float moveDistance = 5f;
    
    private Rigidbody2D rb;
    private float currentHealth;
    private float moveTimer;
    private float moveDirection = 1f;
    private Transform player;
    private bool isAlive = true;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = health;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    
    private void Update()
    {
        if (!isAlive)
            return;
        
        if (player == null)
            return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer < detectionRange)
        {
            // Move toward player
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
            
            // Flip sprite based on direction
            GetComponent<SpriteRenderer>().flipX = direction < 0;
        }
        else
        {
            // Patrol movement
            rb.linearVelocity = new Vector2(speed * moveDirection, rb.linearVelocity.y);
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    
    private void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1f);
        }
    }
}
