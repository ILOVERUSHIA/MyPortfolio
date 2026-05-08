using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PinballGameManager : MonoBehaviour
{
    public static PinballGameManager Instance;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int maxBalls = 3;
    [SerializeField] private TextMeshProUGUI scoreText;

    private int currentBalls;
    private int score = 0;

    void Awake() => Instance = this;

    void Start()
    {
        currentBalls = maxBalls;
        SpawnBall(); // ゲーム開始: ボールを初期位置に生成
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI(); // UI更新を呼ぶ
        Debug.Log($"Score: {score}");
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
    }

    public void OnBallLost()
    {
        currentBalls--;
        if (currentBalls > 0)
        {
            SpawnBall(); // ボール再生成
        }
        else
        {
            Debug.Log("Game Over");
            SaveHighScore(); // ハイスコア保存
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name); // リスタート例
        }
    }



    void SpawnBall() => Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
    void SaveHighScore() => PlayerPrefs.SetInt("HighScore", Mathf.Max(score, PlayerPrefs.GetInt("HighScore", 0)));
}
