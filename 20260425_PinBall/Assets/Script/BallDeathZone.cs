using UnityEngine;

public class BallDeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
            PinballGameManager.Instance.OnBallLost();
        }
    }

}
