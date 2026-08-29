using UnityEngine;

public class LaneController3D : MonoBehaviour
{
    [Header("キー設定")]
    public KeyCode targetKey; // インスペクターからDFJKをそれぞれ設定

    [Header("ビジュアル設定")]
    private MeshRenderer meshRenderer;
    private Color originalColor;
    public Color activeColor = Color.white; // 押したときの色

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            // インスタンス化されたマテリアルの初期色を保存
            originalColor = meshRenderer.material.color;
        }
    }

    void Update()
    {
        if (meshRenderer == null) return;

        // キーが押されている間は白く光る
        if (Input.GetKey(targetKey))
        {
            meshRenderer.material.color = activeColor;
        }
        else
        {
            meshRenderer.material.color = originalColor;
        }
    }
}
