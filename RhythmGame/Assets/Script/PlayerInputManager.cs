using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    [Header("判定の許容誤差（秒）")]
    [SerializeField] private float perfectWindow = 0.05f;
    [SerializeField] private float goodWindow = 0.12f;
    [SerializeField] private float badWindow = 0.20f;

    [Header("入力設定")]
    [SerializeField] private KeyCode hitKey = KeyCode.Space;

    private List<NoteMovement> _activeNotes = new List<NoteMovement>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(hitKey))
        {
            CheckHit();
        }
    }

    public void RegisterNote(NoteMovement note)
    {
        _activeNotes.Add(note);
    }

    public void RemoveNoteFromList(NoteMovement note)
    {
        if (_activeNotes.Contains(note))
        {
            _activeNotes.Remove(note);
            Debug.Log("MISS: ノーツを見逃しました");
        }
    }

    private void CheckHit()
    {
        // 💡 修正ポイント：すでに破壊されたNullノーツがリストの先頭にある場合のみ除外する
        while (_activeNotes.Count > 0 && _activeNotes[0] == null)
        {
            _activeNotes.RemoveAt(0);
        }

        if (_activeNotes.Count == 0) return;

        // 今叩くべき一番古い生きたノーツを取得
        NoteMovement targetNote = _activeNotes[0];

        float currentSongTime = BeatManager.Instance.CurrentBeat * (60f / BeatManager.Instance.Bpm);
        float timeDiff = Mathf.Abs(currentSongTime - targetNote.TargetSongTime);
        Vector3 notePosition = targetNote.transform.position;

        // 💡 すでに判定ラインを通り過ぎて後ろにいきすぎているノーツは、
        // 0.5秒の自動MISS判定が下る前であっても、キー入力された時点でMISSとして即処理する
        if (currentSongTime > targetNote.TargetSongTime && timeDiff > badWindow)
        {
            if (ComboEffectManager.Instance != null) ComboEffectManager.Instance.ShowEffect("MISS", notePosition, false);
            HandleNoteHit(targetNote);
            return;
        }

        // 通常のキー入力判定
        if (timeDiff <= perfectWindow)
        {
            Debug.Log("✨ PERFECT ✨");
            if (ComboEffectManager.Instance != null) ComboEffectManager.Instance.ShowEffect("PERFECT", notePosition, true);
            HandleNoteHit(targetNote);
        }
        else if (timeDiff <= goodWindow)
        {
            Debug.Log("👍 GOOD 👍");
            if (ComboEffectManager.Instance != null) ComboEffectManager.Instance.ShowEffect("GOOD", notePosition, true);
            HandleNoteHit(targetNote);
        }
        else if (timeDiff <= badWindow)
        {
            Debug.Log("⚠️ BAD ⚠️");
            if (ComboEffectManager.Instance != null) ComboEffectManager.Instance.ShowEffect("BAD", notePosition, false);
            HandleNoteHit(targetNote);
        }
        else
        {
            // 判定ラインよりも手前（早すぎる）の場合
            if (currentSongTime < targetNote.TargetSongTime)
            {
                Debug.Log("TOO EARLY (早すぎます)");
            }
        }
    }

    private void HandleNoteHit(NoteMovement note)
    {
        if (_activeNotes.Contains(note))
        {
            _activeNotes.Remove(note);
        }
        note.OnHit();
    }
}
