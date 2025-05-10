using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DepthCameraConstraintsManager : MonoBehaviour
{
    [SerializeField]
    private PositionConstraint positionConstraint;

    [SerializeField]
    private RotationConstraint rotationConstraint;

    public void SetPlayerConstraint()
    {
        ConstraintSource playerConstraint = positionConstraint.GetSource(0);
        ConstraintSource cameraConstraint = positionConstraint.GetSource(1);
        playerConstraint.weight = 1.0f;
        cameraConstraint.weight = 0.0f;

        positionConstraint.SetSource(0, playerConstraint);
        positionConstraint.SetSource(1, cameraConstraint);

        rotationConstraint.SetSource(0, playerConstraint);
        rotationConstraint.SetSource(1, cameraConstraint);
    }

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
