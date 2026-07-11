using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Life System")]
    public int maxDirectionTarget = 3;
    private int currentLife;
    private float damageCooldown = 1f; // 連続ダメージ防止（1秒）
    private float lastDamageTime;

    [Header("Shooting & Aim")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform aimArrow; // 子オブジェクトの矢印スプライト
    public float baseFireRate = 0.5f; // 発射間隔（秒）
    private float nextFireTime;
    private Vector3 lookDirection;

    [Header("Level / EXP")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 10;
    public Image expBarImage; // 最初は灰色、満たされると青くなるImage

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentLife = maxDirectionTarget;
        UpdateExpUI();
    }

    private void Update()
    {
        // 1. 移動入力
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 2. マウスの方向を向く（矢印の回転制御）
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        lookDirection = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        aimArrow.rotation = Quaternion.Euler(0, 0, angle - 90f); // スプライトの向きに合わせて調整

        // 3. 右クリック長押しで発射
        if (Input.GetMouseButton(1) && Time.time >= nextFireTime)
        {
            Shoot();
            // レベルが上がるほど発射間隔が短くなる（弾の強化）
            nextFireTime = Time.time + Mathf.Max(0.1f, baseFireRate - (currentLevel - 1) * 0.05f);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet script = bullet.GetComponent<Bullet>();
        if (script != null)
        {
            // レベルが上がるほど弾の威力を強化
            int damage = 1 + (currentLevel - 1);
            script.Setup(lookDirection, damage);
        }
    }

    public void TakeDamage()
    {
        if (Time.time < lastDamageTime + damageCooldown) return;

        currentLife--;
        lastDamageTime = Time.time;

        if (currentLife <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void GainExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            currentLevel++;
            expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f); // 次の必要経験値を増加
        }
        UpdateExpUI();
    }

    private void UpdateExpUI()
    {
        if (expBarImage != null)
        {
            expBarImage.fillAmount = (float)currentExp / expToNextLevel;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}
