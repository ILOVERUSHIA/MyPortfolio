using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 指定された初速度ベクトルでボールを投げる
    public void Launch(Vector2 velocity)
    {
        // 正しいUnity 6仕様：物理挙動を有効にする(Dynamic)
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = velocity;
    }

    // キャッチされた時などに物理を止める
    public void Hold(Transform holdPosition)
    {
        // 正しいUnity 6仕様：スクリプト制御にする(Kinematic)
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        // ==========================================
        // 【追加】ボールの回転速度（角速度）を完全にゼロにする
        // ==========================================
        rb.angularVelocity = 0f;

        transform.position = holdPosition.position;
        transform.SetParent(holdPosition);
    }
}
