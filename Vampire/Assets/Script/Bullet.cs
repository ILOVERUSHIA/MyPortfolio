using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    private Vector3 moveDirection;
    private int damage;

    public void Setup(Vector3 direction, int damageValue)
    {
        moveDirection = direction;
        damage = damageValue;
        Destroy(gameObject, lifeTime); // 画面外対策で自動消滅
    }

    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // 敵に当たったら消滅
        }
    }
}
