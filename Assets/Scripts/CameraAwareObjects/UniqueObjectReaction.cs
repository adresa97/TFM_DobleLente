using log4net.ObjectRenderer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueObjectReaction : CameraAwareObject
{
    [SerializeField]
    private MeshRenderer objRenderer;

    [SerializeField]
    private bool isRealWorld;

    private Dictionary<float, bool> objStatusMap;

    private bool isActive;
    private MaterialPropertyBlock uniqueMaterial;

    protected override void Start()
    {
        base.Start();

        objStatusMap = new Dictionary<float, bool>();
        objStatusMap.Clear();

        isActive = false;
        uniqueMaterial = new MaterialPropertyBlock();

        SetObjectInactive();
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
        if (objStatusMap[timeStamp]) SetObjectActive();
        else SetObjectInactive();
    }

    protected override void WhenKeyNotExists()
    {
        SetObjectInactive();
    }

    private void SetObjectActive()
    {
        if (!isActive)
        {
            uniqueMaterial.SetFloat("_IsObjectActive", 1);
            objRenderer.SetPropertyBlock(uniqueMaterial);

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

    private void SetObjectInactive()
    {
        if (isActive)
        {
            uniqueMaterial.SetFloat("_IsObjectActive", 0);
            objRenderer.SetPropertyBlock(uniqueMaterial);

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

    #region Reset Data

    protected override void OnResetMap()
    {
        SetObjectInactive();
        objStatusMap.Clear();
    }

    #endregion
}
