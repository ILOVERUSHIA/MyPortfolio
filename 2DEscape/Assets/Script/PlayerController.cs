using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float tileSize = 1.0f;       // 1マスのサイズ
    public float moveSpeed = 5.0f;      // 移動するスピード（大きいほど速い）
    public LayerMask obstacleLayer;    // 壁（障害物）のレイヤー

    private EscapeGameManager gameManager;
    private bool isMoving = false;      // 移動中かどうか

    void Start()
    {
        gameManager = FindObjectOfType<EscapeGameManager>();

        // 開始時に初期位置のイベントをチェック（一応）
        CheckGridEvent();
    }

    void Update()
    {
        // クリア後、または「すでに移動中」なら入力を受け付けない
        if (gameManager != null && gameManager.IsGameClear()) return;
        if (isMoving) return;

        // GetAxisRawで矢印キーやWASDの入力をその瞬間だけ取得
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 斜め移動を禁止（左右の入力を優先）
        if (h != 0) v = 0;

        // 入力があった場合
        if (h != 0 || v != 0)
        {
            Vector2 direction = new Vector2(h, v);

            // 移動先を計算
            Vector2 targetPosition = (Vector2)transform.position + direction * tileSize;

            // 移動先に壁（Obstacleレイヤー）がないかチェック
            // ※0.4fは、1マス（1.0f）より少し小さいサイズで中央を狙うための調整値です
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, tileSize, obstacleLayer);

            // 壁がなければ、滑らかな移動処理（コルーチン）を開始
            if (hit.transform == null)
            {
                StartCoroutine(SmoothMove(targetPosition));
            }
        }
    }

    // スーッと滑らかに1マス移動させる処理
    IEnumerator SmoothMove(Vector2 targetPosition)
    {
        isMoving = true; // 移動開始フラグを立てて、次の入力をブロック

        // 目的地の座標にギリギリまで近づくループ処理
        while (Vector2.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // 1フレーム待つ
        }

        // ぴったり目的地の座標に合わせる
        transform.position = targetPosition;

        // 移動が終わったので、足元のギミック（鍵・出口）をチェック
        CheckGridEvent();

        isMoving = false; // 入力受付を再開
    }

    // 1マス進み終わったあとのイベント判定
    void CheckGridEvent()
    {
        if (gameManager == null) return;

        // プレイヤーの足元（中心から半径0.1の円の中）にあるコライダーを検出
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (var col in colliders)
        {
            if (col.CompareTag("Key"))
            {
                gameManager.GetKey();
                Destroy(col.gameObject);
            }
            else if (col.CompareTag("Exit"))
            {
                gameManager.TryEscape();
            }
        }
    }
}

