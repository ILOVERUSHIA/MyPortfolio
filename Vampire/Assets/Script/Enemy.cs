using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private Rigidbody2D rb;

    [Header("Enemy Stats")]
    public float baseSpeed = 3f;
    public int baseHealth = 3;

    private float currentSpeed;
    private int currentHealth;

    [Header("Drop")]
    public GameObject expGemPrefab;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // シーン内のプレイヤーを検索
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        // 時間経過によるパワーアップを適用
        float scale = GameManager.Instance.enemyPowerUpScale;
        currentSpeed = baseSpeed * (1f + (scale - 1f) * 0.2f); // 速度は控えめに強化
        currentHealth = Mathf.RoundToInt(baseHealth * scale);
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // プレイヤーを追いかける移動
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 経験値ジェムをドロップして自身を破棄
        Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
