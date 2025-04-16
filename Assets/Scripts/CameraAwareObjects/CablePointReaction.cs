using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CablePointReaction : CameraAwareObject
{
    private Dictionary<float, bool> objStatusMap;
    private bool isActive;

    protected override void Start()
    {
        base.Start();

        objStatusMap = new Dictionary<float, bool>();
        objStatusMap.Clear();

        isActive = false;
    }

    public bool IsActive()
    {
        return isActive;
    }

    #region Record Phase

    protected override void RecordVisibleState(float timeStamp)
    {
        objStatusMap[timeStamp] = true;
    }

    protected override void RecordNonVisibleState(float timeStamp)
    {
        objStatusMap[timeStamp] = false;
    }

    #endregion

    #region Replay Phase

    protected override bool IsKeyInStateMap(float timeStamp)
    {
        return objStatusMap.ContainsKey(timeStamp);
    }

    protected override void WhenKeyExists(float timeStamp)
    {
        if (objStatusMap[timeStamp])
        {
            if (!isActive) isActive = true;
        }
        else if (isActive)
        {
            isActive = false;
        }
    }

    protected override void WhenKeyNotExists()
    {
        if (isActive) isActive = false;
    }

    #endregion

    #region Reset Data

    protected override void OnResetMap()
    {
        if (isActive) isActive = false;
        objStatusMap.Clear();
    }

    #endregion
}
