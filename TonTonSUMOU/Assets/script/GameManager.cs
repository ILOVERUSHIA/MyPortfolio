using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Rigidbody stageRb;
    public float shakeForce = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShakeStage();
        }
    }

    void ShakeStage()
    {
        // 瞬間的な上方向の力（振動）を加える
        stageRb.AddForce(Vector3.up * shakeForce, ForceMode.Impulse);

        // わずかなランダム回転の力を加える（トルク）
        stageRb.AddTorque(new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), ForceMode.Impulse);
    }
}
