using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager3D : MonoBehaviour
{
    public static GameManager3D Instance { get; private set; }

    [Header("キー配列設定 (デフォルト: D, F, J, K)")]
    public List<KeyCode> laneKeys = new List<KeyCode> { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

    [Header("判定用リスト (各レーンの判定ラインの3D位置)")]
    public List<Transform> laneDetectors; // 各レーンの判定ラインオブジェクト (4つ)
    private List<Queue<GameObject>> notesInLanes = new List<Queue<GameObject>>();

    [Header("判定の許容時間 (秒)")]
    public float perfectWindow = 0.05f;
    public float greatWindow = 0.10f;
    public float goodWindow = 0.15f;

    [Header("UIテキスト (TextMeshPro)")]
    public TextMeshProUGUI judgementText; // Perfect などの文字用
    public TextMeshProUGUI comboText;     // コンボ数用

    [Header("パーティクル (ParticleSystem)")]
    public ParticleSystem perfectParticle;
    public ParticleSystem greatParticle;
    public ParticleSystem goodParticle;

    private int currentCombo = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 4レーン分のノーツキューを初期化
        for (int i = 0; i < 4; i++)
        {
            notesInLanes.Add(new Queue<GameObject>());
        }

        UpdateComboUI();
        judgementText.text = "";
    }

    void Update()
    {
        // 4つのレーンの入力を監視
        for (int i = 0; i < laneKeys.Count; i++)
        {
            if (Input.GetKeyDown(laneKeys[i]))
            {
                CheckHit(i);
            }
        }
    }

    // ノーツが生成された時、または判定エリア手前に入った時にキューに登録
    public void RegisterNoteInLane(int lane, GameObject note)
    {
        notesInLanes[lane].Enqueue(note);
    }

    // キーが押されたときの判定処理
    void CheckHit(int laneIndex)
    {
        if (notesInLanes[laneIndex].Count == 0) return; // レーンにノーツがなければ無視

        GameObject targetNote = notesInLanes[laneIndex].Peek();
        if (targetNote == null)
        {
            notesInLanes[laneIndex].Dequeue();
            return;
        }

        // 【3D変更点】Z軸（奥行き）のふたつの位置の差を絶対値で計算
        float distance = Mathf.Abs(targetNote.transform.position.z - laneDetectors[laneIndex].position.z);
        float noteSpeed = targetNote.GetComponent<NoteController3D>().scrollSpeed;

        // 判定ラインとの距離を速度で割って「何秒ズレているか」を計算
        float timeDiff = distance / noteSpeed;

        if (timeDiff <= perfectWindow)
        {
            ApplyJudgement("PERFECT", Color.magenta, perfectParticle, true); // TextMeshPro側で虹色グラデーションにするのがオススメ
            RemoveNote(laneIndex, targetNote);
        }
        else if (timeDiff <= greatWindow)
        {
            ApplyJudgement("GREAT", Color.yellow, greatParticle, true);
            RemoveNote(laneIndex, targetNote);
        }
        else if (timeDiff <= goodWindow)
        {
            ApplyJudgement("GOOD", new Color(0.5f, 1f, 0f), goodParticle, true); // 黄緑色
            RemoveNote(laneIndex, targetNote);
        }
    }

    // ミス時の処理（ノーツが通り過ぎたとき）
    public void MissNote(int laneIndex)
    {
        if (notesInLanes[laneIndex].Count > 0)
        {
            notesInLanes[laneIndex].Dequeue();
        }
        ApplyJudgement("MISS", Color.gray, null, false);
    }

    void RemoveNote(int laneIndex, GameObject note)
    {
        notesInLanes[laneIndex].Dequeue();
        Destroy(note);
    }

    // 判定結果を画面と演出に適用
    void ApplyJudgement(string text, Color textColor, ParticleSystem particle, bool isHit)
    {
        judgementText.text = text;
        judgementText.color = textColor;

        if (isHit)
        {
            currentCombo++;
            if (particle != null)
            {
                particle.Play();
            }
        }
        else
        {
            currentCombo = 0;
        }

        UpdateComboUI();
    }

    // コンボUIの表示切り替え
    void UpdateComboUI()
    {
        if (currentCombo == 0)
        {
            comboText.text = "";
        }
        else
        {
            comboText.text = $"{currentCombo} combo";
        }
    }
}
