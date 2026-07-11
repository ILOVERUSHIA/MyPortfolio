using UnityEngine;


public class ExpGem : MonoBehaviour
{
    [Header("獲得できる経験値量")]
    public int expValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 触れた相手が「Player」のタグを持っているかチェック
        if (collision.CompareTag("Player"))
        {
            // 相手からPlayerControllerコンポーネントを取得
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                // プレイヤーの経験値増加メソッドを呼び出す（引数はexpValueのみ）
                player.GainExp(expValue);
            }

            // ジェム自身を削除
            Destroy(gameObject);
        }
    }
}
