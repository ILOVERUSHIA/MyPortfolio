using UnityEngine;
using TMPro;

public class ComboEffectManager : MonoBehaviour
{
    public static ComboEffectManager Instance { get; private set; }

    [Header("UI設定")]
    [SerializeField] private TextMeshProUGUI judgementText;
    [SerializeField] private TextMeshProUGUI comboText; // 💡 追加：コンボ数を表示するText(TMP)
    [SerializeField] private Color perfectColor = Color.yellow;
    [SerializeField] private Color goodColor = Color.cyan;
    [SerializeField] private Color badColor = Color.gray;

    [Header("エフェクト設定")]
    [SerializeField] private ParticleSystem hitEffectPrefab;

    // 💡 現在のコンボ数を管理する変数
    private int _currentCombo = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (judgementText != null) judgementText.text = "";
        if (comboText != null) comboText.text = ""; // 💡 初期状態は非表示
    }

    // 💡 判定文字を表示し、指定位置にエフェクトを出す（引数に「コンボを継続するか」のフラグを追加）
    public void ShowEffect(string rating, Vector3 effectPosition, bool isComboIncrement)
    {
        // 1. コンボ数の計算と表示
        if (isComboIncrement)
        {
            _currentCombo++;
            UpdateComboUI();
        }
        else
        {
            _currentCombo = 0; // BADやMISSの時はコンボリセット
            UpdateComboUI();
        }

        // 2. 判定文字の表示と色の変更
        if (judgementText != null)
        {
            judgementText.text = rating;

            if (rating == "PERFECT") judgementText.color = perfectColor;
            else if (rating == "GOOD") judgementText.color = goodColor;
            else if (rating == "BAD") judgementText.color = badColor;
            else if (rating == "MISS") judgementText.color = Color.red; // 💡 MISSの色を追加

            CancelInvoke(nameof(HideJudgementText));
            Invoke(nameof(HideJudgementText), 0.5f);
        }

        // 3. タップ位置にエフェクトを生成
        if (hitEffectPrefab != null && rating != "MISS") // MISSの時はエフェクトを出さない
        {
            ParticleSystem effect = Instantiate(hitEffectPrefab, effectPosition, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, 2f);
        }
    }

    // 💡 コンボUIの表示更新
    private void UpdateComboUI()
    {
        if (comboText == null) return;

        if (_currentCombo > 0)
        {
            comboText.text = $"{_currentCombo} COMBO";
        }
        else
        {
            comboText.text = ""; // 0コンボの時は表示を消す（または「0 COMBO」にする場合はお好みで）
        }
    }

    // 💡 外部から直接コンボをリセットするための公開メソッド（MISS用）
    public void ResetCombo()
    {
        _currentCombo = 0;
        UpdateComboUI();
    }

    private void HideJudgementText()
    {
        if (judgementText != null) judgementText.text = "";
    }
}
