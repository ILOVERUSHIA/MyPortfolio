using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーンのリロード（再プレイ）に必要
using TMPro;

public class EscapeGameManager : MonoBehaviour
{
    [Header("UI設定")]
    public TextMeshProUGUI infoText;
    public GameObject keyIcon;
    public GameObject restartButton; // 再プレイボタンのオブジェクト

    [Header("演出設定")]
    public float textSpeed = 0.05f;
    public float displayDuration = 5.0f;

    [Header("ゲーム状態")]
    private bool hasKey = false;
    private bool isClear = false;

    private Coroutine textCoroutine;

    void Start()
    {
        if (keyIcon != null) keyIcon.SetActive(false);

        // 開始時は再プレイボタンを隠しておく
        if (restartButton != null) restartButton.SetActive(false);

        ShowMessage("鍵を拾って脱出しろ");
    }

    void Update()
    {
        // Rキーが押されたら、いつでもリスタート関数を実行
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void GetKey()
    {
        if (hasKey) return;

        hasKey = true;
        ShowMessage("鍵を手に入れた！脱出口へ向かえ");

        if (keyIcon != null) keyIcon.SetActive(true);
    }

    public void TryEscape()
    {
        if (hasKey && !isClear)
        {
            isClear = true;
            ShowMessage("ゲームクリア");

            // ゲームクリア時のみ、再プレイボタンを表示する
            if (restartButton != null)
            {
                restartButton.SetActive(true);
            }
        }
        else if (!hasKey)
        {
            ShowMessage("鍵がないと脱出できない！");
        }
    }

    public bool IsGameClear()
    {
        return isClear;
    }

    private void ShowMessage(string message)
    {
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }

        textCoroutine = StartCoroutine(TypeTextRoutine(message));
    }

    private IEnumerator TypeTextRoutine(string targetText)
    {
        if (infoText == null) yield break;

        infoText.text = "";

        // 1文字ずつ表示
        foreach (char letter in targetText.ToCharArray())
        {
            infoText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        // ★「ゲームクリア」の場合は、ここでコルーチンを終了して文字を残す
        if (targetText == "ゲームクリア")
        {
            yield break;
        }

        // それ以外の通常メッセージは、5秒待ってから消す
        yield return new WaitForSeconds(displayDuration);

        // 1文字ずつ消去
        while (infoText.text.Length > 0)
        {
            infoText.text = infoText.text.Substring(0, infoText.text.Length - 1);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    // 再プレイ処理（ボタンとRキーの両方から呼び出される）
    public void RestartGame()
    {
        // 現在開いているシーンの名前を取得して、そのまま再読み込み（リセット）する
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
