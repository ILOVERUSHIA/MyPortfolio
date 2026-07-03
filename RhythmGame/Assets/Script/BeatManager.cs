using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("オーディオ設定")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float bpm = 120f;
    [SerializeField] private float startDelay = 2f; // 💡 曲が鳴るまでの「ノーツが降る猶予時間（秒）」

    public static BeatManager Instance { get; private set; }
    public float Bpm => bpm;
    public float CurrentBeat { get; private set; }
    public bool IsPlaying { get; private set; }

    private double _songStartTime;
    private float _secPerBeat;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _secPerBeat = 60f / bpm;

        // ゲーム開始から startDelay秒後 にBGMが鳴るように正確に予約
        _songStartTime = AudioSettings.dspTime + startDelay;
        audioSource.PlayScheduled(_songStartTime);

        // 💡 ゲーム開始直後からノーツの移動計算を開始するため、フラグはtrueにする
        IsPlaying = true;
    }

    private void Update()
    {
        if (!IsPlaying) return;

        // 💡 修正ポイント：曲の開始時間を「基準（0秒）」として経過時間を計算
        // 曲が鳴る前は、この値が「マイナス（例：-2.0秒）」になります
        double songPosition = AudioSettings.dspTime - _songStartTime;

        // 現在の拍数を計算（曲が鳴る前は、マイナスの拍数「例：-4拍目」になる）
        CurrentBeat = (float)(songPosition / _secPerBeat);
    }
}
