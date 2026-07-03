using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("ノーツ設定")]
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform judgeLine;

    [Header("ギミック設定")]
    [SerializeField] private float noteSpeed = 5f;
    [SerializeField] private int beatInterval = 1;

    // 💡 追加：MISS判定になるまでの猶予時間（インスペクターから変更可能）
    [SerializeField] private float missWindowSeconds = 0.5f;

    private int _nextTargetBeat = 0;
    private float _timeToTarget;

    private void Start()
    {
        float distance = Mathf.Abs(spawnPoint.position.y - judgeLine.position.y);
        _timeToTarget = distance / noteSpeed;
    }

    private void Update()
    {
        if (BeatManager.Instance == null || !BeatManager.Instance.IsPlaying) return;

        float currentBeat = BeatManager.Instance.CurrentBeat;
        float bpm = BeatManager.Instance.Bpm;
        float secPerBeat = 60f / bpm;

        float spawnBeatOffset = _timeToTarget / secPerBeat;
        float currentSpawnBeat = currentBeat + spawnBeatOffset;

        if (currentSpawnBeat >= _nextTargetBeat)
        {
            SpawnNote(_nextTargetBeat);
            _nextTargetBeat += beatInterval;
        }
    }

    private void SpawnNote(int targetBeat)
    {
        GameObject noteObj = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);

        if (noteObj.TryGetComponent<NoteMovement>(out var movement))
        {
            // 💡 修正ポイント：引数に missWindowSeconds を追加して値を渡す
            movement.Initialize(targetBeat, noteSpeed, judgeLine.position.y, missWindowSeconds);
        }

        if (PlayerInputManager.Instance != null)
        {
            PlayerInputManager.Instance.RegisterNote(movement);
        }
    }
}
