using System.Collections.Generic;
using UnityEngine;

public class UniqueMovingReaction : CameraAwareObject
{
    [SerializeField]
    private MeshRenderer[] objRenderers;

    [SerializeField]
    private OneWayMovingInteractee movement;

    [SerializeField]
    private bool isRealWorld;

    private Dictionary<float, bool> objStatusMap;
    private Dictionary<float, Vector3> objPositionMap;

    private bool isActive;
    private MaterialPropertyBlock uniqueMaterial;

    protected override void Start()
    {
        base.Start();

        objStatusMap = new Dictionary<float, bool>();
        objStatusMap.Clear();

        objPositionMap = new Dictionary<float, Vector3>();
        objPositionMap.Clear();

        isActive = false;
        uniqueMaterial = new MaterialPropertyBlock();

        SetObjectInactive();
    }

    #region Record Phase

    protected override void RecordVisibleState(float timeStamp)
    {
        objStatusMap[timeStamp] = true;
        objPositionMap[timeStamp] = transform.position;
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
            SetObjectActive();
            movement.enabled = false;
            transform.position = objPositionMap[timeStamp];
        }
        else
        {
            SetObjectInactive();
            movement.enabled = true;
            movement.OccultMovingStopBeingReplayed();
        }
    }

    protected override void WhenKeyNotExists()
    {
        SetObjectInactive();
        movement.enabled = true;
        movement.OccultMovingStopBeingReplayed();
    }

    private void SetObjectActive()
    {
        if (!isActive)
        {
            if (objRenderers.Length > 0)
            {
                uniqueMaterial.SetFloat("_IsObjectActive", 1);
                for(int i = 0; i < objRenderers.Length; i++)
                {
                    objRenderers[i].SetPropertyBlock(uniqueMaterial);
                }
            }

            if (isRealWorld)
            {
                gameObject.layer = LayerMask.NameToLayer("FromRealWorld");
                for (int i = 0; i < objRenderers.Length; i++)
                {
                    objRenderers[i].gameObject.layer = LayerMask.NameToLayer("FromRealWorld");
                }
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("FromOtherWorld");
                for (int i = 0; i < objRenderers.Length; i++)
                {
                    objRenderers[i].gameObject.layer = LayerMask.NameToLayer("FromOtherWorld");
                }
            }

            isActive = true;
        }
    }

    private void SetObjectInactive()
    {
        if (isActive)
        {
            if (objRenderers.Length > 0)
            {
                uniqueMaterial.SetFloat("_IsObjectActive", 0);
                for (int i = 0; i < objRenderers.Length; i++)
                {
                    objRenderers[i].SetPropertyBlock(uniqueMaterial);
                }
            }

            if (isRealWorld)
            {
                gameObject.layer = LayerMask.NameToLayer("RealWorld");
                for (int i = 0; i < objRenderers.Length; i++)
                {
                    objRenderers[i].gameObject.layer = LayerMask.NameToLayer("RealWorld");
                }
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("OtherWorld");
                for (int i = 0; i < objRenderers.Length; i++)
                {
                    objRenderers[i].gameObject.layer = LayerMask.NameToLayer("OtherWorld");
                }
            }

            isActive = false;
        }
    }

    #endregion

    #region Reset Data

    protected override void OnResetMap()
    {
        SetObjectInactive();
        movement.enabled = true;
        movement.OccultMovingStopBeingReplayed();
        objStatusMap.Clear();
    }

    #endregion
}
