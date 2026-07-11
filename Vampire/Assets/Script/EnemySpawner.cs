using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyPrefab; // 敵のプレハブ

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f; // 敵が湧く間隔（秒）
    public float extraPadding = 2f;    // カメラ外のどのくらい離れた場所に湧かせるか（調整用）

    private Camera mainCamera;
    private float nextSpawnTime;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera == null) return;

        // 時間経過で敵を生成
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemyOutsideCamera();
            // ゲームの経過時間（分）に応じて、少しずつ湧き速度をアップさせる（上限0.2秒間隔）
            float currentInterval = Mathf.Max(0.2f, spawnInterval - (GameManager.Instance.gameTimer / 60f) * 0.1f);
            nextSpawnTime = Time.time + currentInterval;
        }
    }

    private void SpawnEnemyOutsideCamera()
    {
        // 画面の上下左右、どこの外側（4つのエリア）に湧かせるかをランダムで決定
        // 0: 上, 1: 下, 2: 左, 3: 右
        int side = Random.Range(0, 4);

        Vector3 spawnPosition = Vector3.zero;

        // カメラが写している世界（ワールド座標）の端を取得
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        Vector3 camPos = mainCamera.transform.position;

        switch (side)
        {
            case 0: // 上の外側
                spawnPosition.x = Random.Range(camPos.x - camWidth, camPos.x + camWidth);
                spawnPosition.y = camPos.y + camHeight + extraPadding;
                break;
            case 1: // 下の外側
                spawnPosition.x = Random.Range(camPos.x - camWidth, camPos.x + camWidth);
                spawnPosition.y = camPos.y - camHeight - extraPadding;
                break;
            case 2: // 左の外側
                spawnPosition.x = camPos.x - camWidth - extraPadding;
                spawnPosition.y = Random.Range(camPos.y - camHeight, camPos.y + camHeight);
                break;
            case 3: // 右の外側
                spawnPosition.x = camPos.x + camWidth + extraPadding;
                spawnPosition.y = Random.Range(camPos.y - camHeight, camPos.y + camHeight);
                break;
        }

        spawnPosition.z = 0f;

        // 敵を生成
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
