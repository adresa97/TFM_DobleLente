using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraAwareObject : MonoBehaviour
{
    [SerializeField]
    private Transform[] visionPoints;
    private LayerMask allMinusPlayerMask;

    protected virtual void Start()
    {
        allMinusPlayerMask = ~LayerMask.GetMask("Player");
    }

    #region Record Phase

    public void RecordState(float timeStamp, Vector3 camPosition)
    {
        if (CheckVisibility(camPosition))
        {
            RecordVisibleState(timeStamp);
        }
        else
        {
            RecordNonVisibleState(timeStamp);
        }
    }

    private bool CheckVisibility(Vector3 camPosition)
    {
        for (int i = 0; i < visionPoints.Length; i++)
        {
            Vector3 distance = camPosition - visionPoints[i].position;

            RaycastHit hit;
            if (Physics.Raycast(visionPoints[i].position, distance.normalized, out hit, distance.magnitude + 1.0f, allMinusPlayerMask))
            {
                if (hit.collider.tag == "DepthCameraRaycast") return true;
            }
        }

        return false;
    }

    protected abstract void RecordVisibleState(float timeStamp);
    protected abstract void RecordNonVisibleState(float timeStamp);

    #endregion

    #region Replay Phase

    public void ReplayState(float timeStamp)
    {
        if (IsKeyInStateMap(timeStamp))
        {
            WhenKeyExists(timeStamp);
        }
        else
        {
            WhenKeyNotExists();
        }
    }

    protected abstract bool IsKeyInStateMap(float timeStamp);
    protected abstract void WhenKeyExists(float timeStamp);
    protected abstract void WhenKeyNotExists();

    #endregion

    #region Reset Data

    public void ResetMap()
    {
        OnResetMap();
    }

    protected abstract void OnResetMap();

    #endregion
}