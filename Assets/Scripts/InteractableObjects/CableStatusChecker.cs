using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableStatusChecker : MonoBehaviour
{
    [SerializeField]
    private CablePointReaction inCablePoint;

    [SerializeField]
    private CablePointReaction outCablePoint;

    public bool IsCableWorking()
    {
        return inCablePoint.IsActive() && !outCablePoint.IsActive();
    }
}
