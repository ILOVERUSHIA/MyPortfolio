using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public float gameTimer = 0f;
    private const float CLEAR_TIME = 600f; // 10分（600秒）
    private bool isGameActive = true;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;
    public Button replayButtonGameOver;
    public Button replayButtonGameClear;

    [Header("Enemy Difficulty")]
    public float enemyPowerUpScale = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // UIを初期化
        gameOverPanel.SetActive(false);
        gameClearPanel.SetActive(false);

        // リプレイボタンのイベント登録
        replayButtonGameOver.onClick.AddListener(RestartGame);
        replayButtonGameClear.onClick.AddListener(RestartGame);

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (!isGameActive) return;

        // 時間経過と敵のパワーアップ係数計算
        gameTimer += Time.deltaTime;
        enemyPowerUpScale = 1f + (gameTimer / 60f) * 0.2f; // 1分ごとに20%強化

        // 10分経過でゲームクリア
        if (gameTimer >= CLEAR_TIME)
        {
            GameClear();
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        Time.timeScale = 0f; // ゲーム一時停止
        gameOverPanel.SetActive(true);
    }

    private void GameClear()
    {
        isGameActive = false;
        Time.timeScale = 0f; // ゲーム一時停止
        gameClearPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
