using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 【追加】シーン（ステージ）を再読み込みするために必要

public class RikishiDetector : MonoBehaviour
{
    private static bool isGameOver = false;

    public GameObject resultCanvas;
    public TextMeshProUGUI resultText;

    void Start()
    {
        isGameOver = false;
        Time.timeScale = 1f;
    }

    void OnCollisionStay(Collision collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.GetComponent<RikishiDetector>() == null)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.point.y > transform.position.y - 0.3f)
                {
                    Debug.Log("【決着】 " + gameObject.name + " の負け（転倒）！");
                    DetermineWinner(gameObject.name);
                    break;
                }
            }
        }
    }

    void Update()
    {
        // 【追加】ゲームオーバー状態で画面がタップされたら、今のシーンを最初からやり直す
        if (isGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 現在開いているシーンの名前を取得して、再読み込みする
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        if (transform.position.y < -1f)
        {
            Debug.Log("【決着】 " + gameObject.name + " の負け（転落）！");
            DetermineWinner(gameObject.name);
        }
    }

    void DetermineWinner(string loserName)
    {
        isGameOver = true;
        Time.timeScale = 0.2f;

        if (resultCanvas != null && resultText != null)
        {
            resultCanvas.SetActive(true);

            if (loserName == "CPU")
            {
                resultText.text = "WIN";
                resultText.color = Color.yellow;
            }
            else
            {
                resultText.text = "LOSE";
                resultText.color = Color.red;
            }
        }
    }
}
