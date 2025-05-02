using log4net.ObjectRenderer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueSwitchReaction : CameraAwareObject
{
    [SerializeField]
    private bool isRealWorld;

    private Dictionary<float, bool> objStatusMap;

    private bool isActive;

    protected override void Start()
    {
        base.Start();

        objStatusMap = new Dictionary<float, bool>();
        objStatusMap.Clear();

        isActive = false;

        SetSwitchIdlePreviewMode();
    }

    #region Record Phase

    protected override void RecordVisibleState(float timeStamp)
    {
        objStatusMap[timeStamp] = true;
        SetSwitchRecordReplayMode();
    }

    protected override void RecordNonVisibleState(float timeStamp)
    {
        objStatusMap[timeStamp] = false;
        SetSwitchIdlePreviewMode();
    }

    private void SetSwitchRecordReplayMode()
    {
        if (!isActive)
        {
            if (isRealWorld)
            {
                gameObject.layer = LayerMask.NameToLayer("FromRealWorld");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("FromOtherWorld");
            }

            isActive = true;
        }
    }

    private void SetSwitchIdlePreviewMode()
    {
        if (isActive)
        {
            if (isRealWorld)
            {
                gameObject.layer = LayerMask.NameToLayer("RealWorld");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("OtherWorld");
            }

            isActive = false;
        }
    }

    #endregion

    #region Replay Phase

    protected override bool IsKeyInStateMap(float timeStamp)
    {
        return objStatusMap.ContainsKey(timeStamp);
    }

    protected override void WhenKeyExists(float timeStamp)
    {
        if (objStatusMap[timeStamp]) SetSwitchRecordReplayMode();
        else SetSwitchIdlePreviewMode();
    }

    protected override void WhenKeyNotExists()
    {
        SetSwitchIdlePreviewMode();
    }

    #endregion

    #region Reset Data

    protected override void OnResetMap()
    {
        SetSwitchIdlePreviewMode();
        objStatusMap.Clear();
    }

    #endregion
}
