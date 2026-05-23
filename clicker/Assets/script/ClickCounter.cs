using UnityEngine;
using TMPro; // TextMeshProを使用するため

public class ClickCounter : MonoBehaviour
{
    // クリック数を表示するUIテキスト
    public TextMeshProUGUI countText;

    // クリック数をカウントする変数
    private int clickCount = 0;

    void Start()
    {
        // 初期状態のクリック数を表示
        UpdateCountText();
    }

    void Update()
    {
        // 左クリック（マウスボタン0）された瞬間
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++; // クリック数を1増やす
            UpdateCountText(); // テキストを更新
        }
    }

    // UIのテキストを書き換えるメソッド
    void UpdateCountText()
    {
        countText.text = "click: " + clickCount.ToString();
    }
}
