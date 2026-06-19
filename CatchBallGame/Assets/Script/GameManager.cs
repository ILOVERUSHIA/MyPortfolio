using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GamePhase { PlayerTurn, BallInAirToOpponent, OpponentTurn, BallInAirToPlayer, GameOver }
    public GamePhase currentPhase;

    [Header("Characters")]
    public GameObject player;
    public GameObject opponent;
    public Ball ball;

    [Header("Move Settings")]
    public float playerMoveSpeed = 5f;

    private BallLauncher playerLauncher;
    private BallLauncher opponentLauncher;
    private Vector2 currentVelocity;

    private void Start()
    {
        playerLauncher = player.GetComponent<BallLauncher>();
        opponentLauncher = opponent.GetComponent<BallLauncher>();

        // 最初はプレイヤーがボールを持っている状態からスタート
        currentPhase = GamePhase.PlayerTurn;
        ball.Hold(playerLauncher.handPosition);
    }

    private void Update()
    {
        switch (currentPhase)
        {
            case GamePhase.PlayerTurn:
                HandlePlayerTurn();
                HandlePlayerMovement();
                break;

            case GamePhase.BallInAirToOpponent:
                // ボールが相手に飛んでいる間（判定はCollider側で行う）
                break;

            case GamePhase.OpponentTurn:
                // 【安全対策】
                // フェーズの変更をコルーチン内に任せることで、二重呼び出しを完全に防ぎます
                currentPhase = GamePhase.BallInAirToPlayer;
                StartCoroutine(OpponentRoutine());
                break;

            case GamePhase.BallInAirToPlayer:
                // 相手が投げたボールを待つ間、プレイヤーは左右移動ができる
                HandlePlayerMovement();
                break;

            case GamePhase.GameOver:
                Debug.Log("ゲームオーバー！落球しました。");
                break;
        }
    }

    // 1. プレイヤーの投球操作
    private void HandlePlayerTurn()
    {
        // 右クリック長押しで狙いを定める
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentVelocity = playerLauncher.CalculateVelocity(mousePos);
            playerLauncher.DrawTrajectory(currentVelocity);

            // 狙っている最中に左クリックで発射
            if (Input.GetMouseButtonDown(0))
            {
                playerLauncher.ClearTrajectory();
                ball.transform.SetParent(null);
                ball.Launch(currentVelocity);
                currentPhase = GamePhase.BallInAirToOpponent;
            }
        }

        // 右クリックを離したら予測線を消す
        if (Input.GetMouseButtonUp(1))
        {
            playerLauncher.ClearTrajectory();
        }
    }

    // 2. 相手（NPC）の自動投球ルーチン
    private IEnumerator OpponentRoutine()
    {
        yield return new WaitForSeconds(1.0f); // キャッチ後、1秒待ってから狙いを定める

        // プレイヤーの現在位置を基準に狙いを決める（ブレを入れる）
        Vector3 targetPos = player.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(2f, 5f), 0);

        // カメラ外に投げないように制限する処理
        if (Camera.main != null)
        {
            float minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            float maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

            float screenPadding = 1.5f;
            float clampedTargetX = Mathf.Clamp(targetPos.x, minX + screenPadding, maxX - screenPadding);

            targetPos.x = clampedTargetX;
        }

        Vector2 oppVelocity = opponentLauncher.CalculateVelocity(targetPos);

        // 相手の予測線を1.5秒間表示してプレイヤーに見せる
        opponentLauncher.DrawTrajectory(oppVelocity);
        yield return new WaitForSeconds(1.5f);

        // 投げる
        opponentLauncher.ClearTrajectory();
        ball.transform.SetParent(null);
        ball.Launch(oppVelocity);
    }

    // 3. 相手の投球中や自分のターン中にプレイヤーを左右に動かす
    private void HandlePlayerMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // A/D または 左右矢印キー
        player.transform.Translate(Vector3.right * moveInput * playerMoveSpeed * Time.deltaTime);

        // カメラ外にプレイヤーが出ないように制限する処理
        if (Camera.main != null)
        {
            float minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            float maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

            float padding = 0.5f;
            float clampedX = Mathf.Clamp(player.transform.position.x, minX + padding, maxX - padding);

            player.transform.position = new Vector3(clampedX, player.transform.position.y, player.transform.position.z);
        }
    }

    // キャッチ成否の判定メソッド
    public void BallCaught(bool isPlayer)
    {
        if (isPlayer && currentPhase == GamePhase.BallInAirToPlayer)
        {
            ball.Hold(playerLauncher.handPosition);
            currentPhase = GamePhase.PlayerTurn;
        }
        else if (!isPlayer && currentPhase == GamePhase.BallInAirToOpponent)
        {
            ball.Hold(opponentLauncher.handPosition);
            currentPhase = GamePhase.OpponentTurn;
        }
    }

    // ボールが地面に落ちた時に呼ばれるメソッド
    public void BallDropped()
    {
        // すでにボールが空中以外（戻り処理中など）なら何度も実行しないようにガード
        if (currentPhase != GamePhase.BallInAirToOpponent && currentPhase != GamePhase.BallInAirToPlayer)
        {
            return;
        }

        // 状態を一度リセット（相手の投球ルーチンなどを止める）
        StopAllCoroutines();
        if (opponentLauncher != null) opponentLauncher.ClearTrajectory();
        if (playerLauncher != null) playerLauncher.ClearTrajectory();

        // 【変更点】即座に戻さず、時間差で戻すコルーチンをスタート
        StartCoroutine(DelayResetRoutine());
    }

    // 【追加】地面を少し転がってから手元に戻る時間差ルーチン
    private IEnumerator DelayResetRoutine()
    {
        // 状態を一時的に待機状態にする（プレイヤーの移動などは可能）
        currentPhase = GamePhase.GameOver;

        Debug.Log("落球！少し転がします...");

        // ⏳ ここで転がる時間を調整できます（1.0f = 1秒間転がす）
        yield return new WaitForSeconds(1.0f);

        // 1秒経ったら手元に戻す
        ResetBallToPlayer();
    }

    // ボールをプレイヤーの手元に戻してリスタートする処理
    private void ResetBallToPlayer()
    {
        // ボールをプレイヤーの手元（Hand）に固定する（物理を止める）
        ball.Hold(playerLauncher.handPosition);

        // フェーズをプレイヤーのターンに戻して再開
        currentPhase = GamePhase.PlayerTurn;

        Debug.Log("ボールがプレイヤーの手元に戻りました。");
    }
}
