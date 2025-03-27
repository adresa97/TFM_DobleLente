using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DepthCameraConstraintsManager : MonoBehaviour
{
    [SerializeField]
    PositionConstraint positionConstraint;

    [SerializeField]
    RotationConstraint rotationConstraint;

    public void ActivateConstraint()
    {
        positionConstraint.constraintActive = true;
        rotationConstraint.constraintActive = true;
    }

    public void DeactivateConstraint()
    {
        positionConstraint.constraintActive = false;
        rotationConstraint.constraintActive = false;
    }
}
