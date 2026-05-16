using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            // カメラの回転（向き）と完全に同期させる
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
