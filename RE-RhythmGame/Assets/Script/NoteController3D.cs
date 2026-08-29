using UnityEngine;

public class NoteController3D : MonoBehaviour
{
    [Header("移動設定")]
    public float scrollSpeed = 10.0f; // ノーツの流れる速度（3Dは速めがおすすめ）
    public int laneIndex;             // 0:D, 1:F, 2:J, 3:K 

    void Update()
    {
        // 3D空間の奥から手前（Vector3.back = Z軸のマイナス方向）へ移動
        transform.Translate(Vector3.back * scrollSpeed * Time.deltaTime, Space.World);
    }

    // 判定ラインを大幅に過ぎた場所に設置した3Dコライダー（MissBoundary）に触れたら自動削除
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MissBoundary"))
        {
            GameManager3D.Instance.MissNote(laneIndex);
            Destroy(gameObject);
        }
    }
}
