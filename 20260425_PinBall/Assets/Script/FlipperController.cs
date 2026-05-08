using UnityEngine;

public class FlipperController : MonoBehaviour
{
    [SerializeField] private KeyCode flipKey;
    [SerializeField] private float hitStrength = 10000f;
    [SerializeField] private float springDamper = 150f;

    private HingeJoint hinge;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        hinge.useSpring = true;
    }

    void Update()
    {
        JointSpring spring = new JointSpring();
        spring.spring = hitStrength;
        spring.damper = springDamper;

        // 入力あり？ -> AddTorque / Rotation で動かす (HingeJoint版)
        if (Input.GetKey(flipKey))
        {
            spring.targetPosition = hinge.limits.max;
        }
        else
        {
            spring.targetPosition = hinge.limits.min;
        }
        hinge.spring = spring;
    }
}
