using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Animations;

public class GrabableObjectBehaviour : MonoBehaviour
{
    [SerializeField]
    private Rigidbody body;

    [SerializeField]
    private PositionConstraint positionConstraint;

    [SerializeField]
    private RotationConstraint rotationConstraint;

    private bool isHold;

    private void Start()
    {
        positionConstraint.constraintActive = false;
        rotationConstraint.constraintActive = false;

        isHold = false;
    }

    public void ActivateConstraint()
    {
        body.excludeLayers = LayerMask.GetMask("Player");
        body.useGravity = false;
        positionConstraint.constraintActive = true;
        rotationConstraint.constraintActive = true;

        isHold = true;
    }

    public void DeactivateConstraint()
    {
        body.excludeLayers = LayerMask.GetMask("");
        body.useGravity = true;
        positionConstraint.constraintActive = false;
        rotationConstraint.constraintActive = false;

        isHold = false;
    }
}
