using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    private int _targetBeat;
    private float _speed;
    private float _judgeLineY;
    private float _startY;
    private float _spawnBeat;
    private float _missWindowSeconds; // 💡 追加：受け取った数値を保持する変数

    public float TargetSongTime { get; private set; }

    // 💡 修正ポイント①：引数に missWindow を追加
    public void Initialize(int targetBeat, float speed, float judgeLineY, float missWindow)
    {
        _targetBeat = targetBeat;
        _speed = speed;
        _judgeLineY = judgeLineY;
        _startY = transform.position.y;
        _missWindowSeconds = missWindow; // 💡 値を代入

        _spawnBeat = BeatManager.Instance.CurrentBeat;

        float secPerBeat = 60f / BeatManager.Instance.Bpm;
        TargetSongTime = _targetBeat * secPerBeat;
    }

    private void Update()
    {
        if (BeatManager.Instance == null) return;

        float currentSongTime = BeatManager.Instance.CurrentBeat * (60f / BeatManager.Instance.Bpm);

        // 💡 修正ポイント②：固定値の 0.5f だった部分を変数に置き換え
        if (currentSongTime > TargetSongTime + _missWindowSeconds)
        {
            TriggerMiss();
            return;
        }

        float totalBeatsToTarget = _targetBeat - _spawnBeat;
        float beatsPassed = BeatManager.Instance.CurrentBeat - _spawnBeat;
        float progress = beatsPassed / totalBeatsToTarget;

        float currentY = Mathf.LerpUnclamped(_startY, _judgeLineY, progress);
        transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
    }

    private void TriggerMiss()
    {
        if (PlayerInputManager.Instance != null)
        {
            PlayerInputManager.Instance.RemoveNoteFromList(this);
        }

        if (ComboEffectManager.Instance != null)
        {
            ComboEffectManager.Instance.ShowEffect("MISS", transform.position, false);
        }

        Destroy(gameObject);
    }

    public void OnHit()
    {
        Destroy(gameObject);
    }
}
