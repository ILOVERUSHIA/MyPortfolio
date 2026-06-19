using UnityEngine;

public class CatchZone : MonoBehaviour
{
    public bool isPlayerZone; // キャラクターに付ける時はチェック、ボールに付ける時はチェックなし
    private GameManager gameManager;

    private void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    // ==========================================
    // 1. トリガー判定（キャラクターがボールをキャッチする用）
    // ==========================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball") && !gameObject.CompareTag("Ball"))
        {
            gameManager.BallCaught(isPlayerZone);
        }
    }

    // ==========================================
    // 2. 【新設】物理衝突判定（ボールが地面にぶつかって転がる用）
    // ==========================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ボール自身が、"Ground" タグの付いた物理地面にぶつかった時
        if (gameObject.CompareTag("Ball") && collision.gameObject.CompareTag("Ground"))
        {
            gameManager.BallDropped();
        }
    }
}
