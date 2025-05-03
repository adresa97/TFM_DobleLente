using UnityEngine;

public class GrabableObjectBehaviour : MonoBehaviour
{
    [SerializeField]
    private Rigidbody body;

    private Transform constraintPoint;
    private bool isHold;

    private void Start()
    {
        isHold = false;
    }

    private void FixedUpdate()
    {
        if (isHold)
        {
            Vector3 speed = (constraintPoint.position - body.position) * (1 / Time.fixedDeltaTime);
            body.velocity = speed;
        }
    }

    public void ActivateConstraint(Transform point)
    {
        body.excludeLayers = LayerMask.GetMask("Player");
        body.useGravity = false;
        body.maxAngularVelocity = 2;
        constraintPoint = point;

        isHold = true;
    }

    public void DeactivateConstraint()
    {
        body.excludeLayers = LayerMask.GetMask("");
        body.useGravity = true;
        body.maxAngularVelocity = 7;
        constraintPoint = null;

        isHold = false;
    }
}
