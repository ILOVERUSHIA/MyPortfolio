using UnityEngine;

public class HammerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float rotationSpeed = 500f; // 回転の強さ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. マウスの世界座標を取得
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // 2. ハンマーからマウスへの方向ベクトルを計算
        Vector2 direction = mousePosition - transform.position;

        // 3. 目標とする角度を計算
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 4. 現在の角度から目標角度への差分を計算
        float angleDifference = Mathf.DeltaAngle(rb.rotation, targetAngle);

        // 5. FixedUpdateで処理するためにトルク（回転力）として加える、または直接速度を変える
        // 簡易版として、ここでは回転速度を直接操作します
        rb.angularVelocity = angleDifference * (rotationSpeed * Time.deltaTime);
    }
}
