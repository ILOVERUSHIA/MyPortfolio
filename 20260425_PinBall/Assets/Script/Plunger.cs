using UnityEngine;

public class Plunger : MonoBehaviour
{
    [SerializeField] private KeyCode launchKey = KeyCode.Space;
    [SerializeField] private float maxForce = 2000f;
    private float currentPower;
    private bool isBallReady;
    private Rigidbody ballRb;

    void Update()
    {
        if (isBallReady && Input.GetKey(launchKey))
        {
            currentPower = Mathf.Min(currentPower + Time.deltaTime * 1000f, maxForce);
        }

        if (Input.GetKeyUp(launchKey) && isBallReady)
        {
            ballRb.AddForce(Vector3.forward * currentPower);
            currentPower = 0;
        }
    }

    private void OnTriggerEnter(Collider other) => SetBall(other, true);
    private void OnTriggerExit(Collider other) => SetBall(other, false);

    void SetBall(Collider other, bool ready)
    {
        if (other.CompareTag("Ball"))
        {
            isBallReady = ready;
            ballRb = ready ? other.GetComponent<Rigidbody>() : null;
        }
    }
}
