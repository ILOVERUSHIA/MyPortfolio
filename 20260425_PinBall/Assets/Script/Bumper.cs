using System.Collections;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [Header("物理設定")]
    [SerializeField] private float bumpForce = 10f;
    [SerializeField] private int scoreValue = 100;

    [Header("演出設定")]
    [SerializeField] private GameObject hitEffect; // パーティクルプレハブ
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private Color hitColor = Color.yellow;

    private Color originalColor;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null) originalColor = rend.material.color;

        GameObject clonePlayer = Instantiate(hitEffect);
        hitEffect.name = hitEffect.name.Replace("(Clone)", "");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // 1. 物理的な反発
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (collision.transform.position - transform.position).normalized;
                rb.AddForce(direction * bumpForce, ForceMode.Impulse);
            }

            // 2. スコア加算
            if (PinballGameManager.Instance != null)
            {
                PinballGameManager.Instance.AddScore(scoreValue);
            }

            // 3. エフェクト生成 (選択されたコードの反映)
            if (hitEffect != null)
            {
                // 衝突した地点（contact[0].point）にエフェクトを出す
                Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
            }

            // 4. 音と色の演出
            if (hitSound) hitSound.Play();
            StopAllCoroutines();
            StartCoroutine(FlashEffect());
        }
    }

    IEnumerator FlashEffect()
    {
        if (rend == null) yield break;
        rend.material.color = hitColor;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
    }
}
