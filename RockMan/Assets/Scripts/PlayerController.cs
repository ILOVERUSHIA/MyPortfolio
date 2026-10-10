using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 2f;
    [SerializeField] private float groundDamping = 0.8f;
    
    [SerializeField] public float slideSpeed = 8f;
    [SerializeField] public float slideDuration = 0.5f;
    [SerializeField] public float slideCooldown = 0.3f;
    
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public float groundCheckDistance = 0.1f;
    [SerializeField] private Vector3 groundCheckOffset = Vector3.zero;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isSliding;
    private float slideTimer;
    private float slideCooldownTimer;
    private int facingDirection = 1; // 1 for right, -1 for left
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on player!");
        }
    }
    
    private void Update()
    {
        HandleInput();
        CheckGrounded();
        UpdateSliding();
        UpdateFacing();
    }
    
    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyDrag();
    }
    
    private void HandleInput()
    {
        // Movement input
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        
        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        
        // Slide input (Shift key while moving horizontally)
        if (Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Abs(moveInput.x) > 0.1f && isGrounded && slideCooldownTimer <= 0f)
        {
            StartSlide();
        }
    }
    
    private void CheckGrounded()
    {
        // Cast a small box downward to check if grounded
        Vector3 checkPos = transform.position + groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
    }
    
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }
    
    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        slideCooldownTimer = slideCooldown + slideDuration;
    }
    
    private void UpdateSliding()
    {
        if (slideCooldownTimer > 0f)
        {
            slideCooldownTimer -= Time.deltaTime;
        }
        
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0f)
            {
                isSliding = false;
            }
        }
    }
    
    private void ApplyMovement()
    {
        float targetSpeed;
        
        if (isSliding)
        {
            // Slide in the direction we're moving
            targetSpeed = slideSpeed * Mathf.Sign(moveInput.x);
        }
        else
        {
            targetSpeed = moveSpeed * moveInput.x;
        }
        
        // Smooth velocity change
        float velocityDiff = targetSpeed - rb.linearVelocity.x;
        rb.AddForce(new Vector2(velocityDiff * groundDamping, 0f), ForceMode2D.Force);
    }
    
    private void ApplyDrag()
    {
        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
            
            // Stop vertical velocity from increasing on ground
            if (rb.linearVelocity.y < 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            }
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }
    
    private void UpdateFacing()
    {
        if (moveInput.x > 0.1f && facingDirection == -1)
        {
            facingDirection = 1;
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < -0.1f && facingDirection == 1)
        {
            facingDirection = -1;
            spriteRenderer.flipX = true;
        }
    }
    
    public int GetFacingDirection()
    {
        return facingDirection;
    }
    
    public bool IsSliding()
    {
        return isSliding;
    }
    
    public bool IsGrounded()
    {
        return isGrounded;
    }
    
    public Vector2 GetVelocity()
    {
        return rb.linearVelocity;
    }
}
