using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [Header("Settings")]
    public float maxForce = 15f; // 最大の投球強さ
    public int resolution = 30;  // 予測線のドット数（滑らかさ）
    public float timeStep = 0.05f; // 予測点の間隔（秒）

    [Header("References")]
    public LineRenderer lineRenderer;
    public Transform handPosition; // ボールを持つ位置

    private float gravity;

    private void Start()
    {
        // Physics2Dの重力加速度を取得 (通常は 9.81)
        gravity = Mathf.Abs(Physics2D.gravity.y);
        if (lineRenderer != null) lineRenderer.positionCount = 0;
    }

    // 入力（マウス位置など）から初速度ベクトルを計算する
    public Vector2 CalculateVelocity(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - handPosition.position);
        // ターゲットへの方向と、設定した最大強さを基準に速度を決定
        float distance = direction.magnitude;
        Vector2 velocity = direction.normalized * Mathf.Min(distance * 2f, maxForce);
        return velocity;
    }

    // 予測線を描画するメソッド
    public void DrawTrajectory(Vector2 velocity)
    {
        // 点々にするために、解像度（点の数）を少し多めの「50」程度に設定すると綺麗に見えます
        int dotResolution = 50;

        lineRenderer.positionCount = dotResolution;
        Vector2 startPosition = handPosition.position;

        for (int i = 0; i < dotResolution; i++)
        {
            float t = i * timeStep;

            // 【点々にするトリック】
            // もし「i」が奇数なら、1つ前の偶数の位置と全く同じ場所に座標を置くことで、
            // その区間の線が縮んで消え、見た目がブツ切りの点線（破線）になります
            int index = i;
            if (index % 2 != 0)
            {
                index = index - 1;
            }

            float adjustedTime = index * timeStep;
            float x = startPosition.x + velocity.x * adjustedTime;
            float y = startPosition.y + (velocity.y * adjustedTime) - (0.5f * gravity * adjustedTime * adjustedTime);

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    // 予測線を消す
    public void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }
}
